using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI TotalScoreText;
    public TextMeshProUGUI WrongLaneScoreText;
    public TextMeshProUGUI CorrectLaneScoreText;
    public TextMeshProUGUI PhoneOpenedText;
    public TextMeshProUGUI TimeSlowText;
    public TextMeshProUGUI TimeStopText;
    public TextMeshProUGUI SpeedPenaltyText;
    public TextMeshProUGUI LivesLeftText;
    public TextMeshProUGUI[] scoreboardEntries;

    private ScoreManager scoreManager;
    private LivesManager livesManager;
    private bool scoreSaved = false;
    private string currentScoreboardType;
    private bool componentsInitialized = false;

    void Awake()
    {
        Debug.Log("[ScoreBoardManager] Awake called");
    }

    private void InitializeComponents()
    {
        // Prevent multiple initializations
        if (componentsInitialized && scoreManager != null && livesManager != null)
        {
            Debug.Log("[ScoreBoardManager] Already initialized, skipping");
            return;
        }

        Debug.Log("[ScoreBoardManager] InitializeComponents called");

        if (player == null)
        {
            Debug.LogError("[ScoreBoardManager] Player GameObject is NULL! You must assign it in the Inspector!");
            return;
        }

        // Check if player is active
        if (!player.activeInHierarchy)
        {
            Debug.LogWarning($"[ScoreBoardManager] Player '{player.name}' is INACTIVE in hierarchy!");
        }

        Debug.Log($"[ScoreBoardManager] Assigned player: {player.name}, Active: {player.activeInHierarchy}");

        scoreManager = player.GetComponent<ScoreManager>();
        livesManager = player.GetComponent<LivesManager>();

        if (scoreManager == null)
        {
            Debug.LogError($"[ScoreBoardManager] ScoreManager NOT FOUND on {player.name}!");
        }
        else
        {
            Debug.Log($"[ScoreBoardManager] ✓ ScoreManager found on {player.name}");
        }

        if (livesManager == null)
        {
            Debug.LogError($"[ScoreBoardManager] LivesManager NOT FOUND on {player.name}!");
        }
        else
        {
            Debug.Log($"[ScoreBoardManager] ✓ LivesManager found on {player.name}");
        }

        if (scoreManager != null && livesManager != null)
        {
            componentsInitialized = true;
            Debug.Log("[ScoreBoardManager] Components successfully initialized");
        }
    }

    void Update()
    {
        // Only update display when scoreboard is active and components are valid
        if (gameObject.activeSelf && livesManager != null && scoreManager != null)
            DisplayScore();
    }

    // Called by OpenScoreBoard when scoreboard becomes visible
    public void RefreshScoreboard(string type, bool isFinalSim)
    {
        Debug.Log($"[ScoreBoardManager] ========== RefreshScoreboard called: type={type}, isFinalSim={isFinalSim} ==========");

        // Reset save flag so we can save this simulation
        scoreSaved = false;

        // Make sure components are initialized
        InitializeComponents();

        // Check if initialization succeeded
        if (livesManager == null || scoreManager == null)
        {
            Debug.LogError("[ScoreBoardManager] CRITICAL: Components are NULL after initialization!");
            if (player != null)
                Debug.LogError($"[ScoreBoardManager] Player is assigned to: {player.name}, but components not found!");
            return;
        }

        Debug.Log($"[ScoreBoardManager] Components ready - Lives: {livesManager.NumLives}, Correct: {scoreManager.SecondsOnCorrectLane}, Wrong: {scoreManager.SecondsOnWrongLane}");

        // Display score immediately (Update() will keep refreshing it)
        DisplayScore();

        // Delay save by 0.1 seconds to let managers populate their values
        Invoke(nameof(SaveScore), 0.1f);

        // Only show leaderboard on final simulation
        if (isFinalSim)
        {
            Debug.Log("[ScoreBoardManager] Final sim - will display leaderboard after save");
            currentScoreboardType = type;
            // Delay leaderboard display to ensure save completes first
            Invoke(nameof(DelayedDisplayScoreboard), 0.2f);
        }
    }

    private void DelayedDisplayScoreboard()
    {
        Debug.Log("[ScoreBoardManager] DelayedDisplayScoreboard executing");
        DisplayScoreboard(currentScoreboardType);
    }

    void SaveScore()
    {
        Debug.Log("[ScoreBoardManager] ========== SaveScore called ==========");

        if (scoreSaved)
        {
            Debug.Log("[ScoreBoardManager] Score already saved, skipping");
            return;
        }

        if (ScoreDataManager.Instance == null)
        {
            Debug.LogError("[ScoreBoardManager] ScoreDataManager.Instance is NULL!");
            return;
        }

        if (livesManager == null || scoreManager == null)
        {
            Debug.LogError("[ScoreBoardManager] livesManager or scoreManager is NULL in SaveScore!");
            return;
        }

        // Read directly from ScoreManager and LivesManager
        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        Debug.Log($"[ScoreBoardManager] VALUES FROM MANAGERS - Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}");

        // Pedestrian mode - check if PhoneOpened field exists
        if (PhoneOpenedText != null)
        {
            int phoneOpened = scoreManager.PhoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);

            Debug.Log($"[ScoreBoardManager] PEDESTRIAN - Phone: {phoneOpened}, Total: {totalScore}");

            bool isFirstSim = PedestrianGameNavigationManager.Instance == null;

            Debug.Log($"[ScoreBoardManager] Saving PEDESTRIAN to CSV: totalScore={totalScore}, lives={livesLeft}, correct={secondsCorrect}, wrong={secondsWrong}, phone={phoneOpened}, isFirst={isFirstSim}");

            ScoreDataManager.Instance.SavePedestrianScore(totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened, isFirstSim);
        }
        // Cyclist mode
        else if (TimeSlowText != null && TimeStopText != null && SpeedPenaltyText != null)
        {
            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;

            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int)(timeToSlow / 0.25f * 2) - (int)(timeToStop / 0.25f * 2) - speedPenalty;

            Debug.Log($"[ScoreBoardManager] CYCLIST - Total: {totalScore}, Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}, TimeSlow: {timeToSlow}, TimeStop: {timeToStop}, Speed: {speedPenalty}");

            bool isFirstSim = GameNavigationManager.Instance == null;

            Debug.Log($"[ScoreBoardManager] Saving CYCLIST to CSV: isFirstSim={isFirstSim}");

            ScoreDataManager.Instance.SaveCyclistScore(totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty, isFirstSim);
        }
        else
        {
            Debug.LogError("[ScoreBoardManager] Could not determine player type! Text fields are null.");
            Debug.LogError($"[ScoreBoardManager] PhoneOpenedText={PhoneOpenedText}, TimeSlowText={TimeSlowText}, TimeStopText={TimeStopText}, SpeedPenaltyText={SpeedPenaltyText}");
        }

        scoreSaved = true;
        Debug.Log("[ScoreBoardManager] ========== Save complete ==========");
    }

    void DisplayScore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (livesManager == null || scoreManager == null)
        {
            // Don't spam errors, just return silently
            return;
        }

        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        // Display ONLY the numbers
        if (LivesLeftText != null)
            LivesLeftText.text = livesLeft.ToString();

        if (CorrectLaneScoreText != null)
            CorrectLaneScoreText.text = secondsCorrect.ToString();

        if (WrongLaneScoreText != null)
            WrongLaneScoreText.text = secondsWrong.ToString();

        // Pedestrian-specific fields
        if (PhoneOpenedText != null)
        {
            int phoneOpened = scoreManager.PhoneOpened;
            PhoneOpenedText.text = phoneOpened.ToString();

            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);

            if (TotalScoreText != null)
                TotalScoreText.text = totalScore.ToString();
        }
        // Cyclist-specific fields
        else if (TimeSlowText != null && TimeStopText != null && SpeedPenaltyText != null)
        {
            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;

            TimeSlowText.text = timeToSlow.ToString("F2");
            TimeStopText.text = timeToStop.ToString("F2");
            SpeedPenaltyText.text = speedPenalty.ToString();

            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int)(timeToSlow / 0.25f * 2) - (int)(timeToStop / 0.25f * 2) - speedPenalty;

            if (TotalScoreText != null)
                TotalScoreText.text = totalScore.ToString();
        }
    }

    public void DisplayScoreboard(string type)
    {
        Debug.Log($"[ScoreBoardManager] ========== DisplayScoreboard called with type: {type} ==========");

        if (scoreboardEntries == null)
        {
            Debug.LogError("[ScoreBoardManager] scoreboardEntries is NULL!");
            return;
        }

        if (scoreboardEntries.Length == 0)
        {
            Debug.LogError("[ScoreBoardManager] scoreboardEntries.Length is 0!");
            return;
        }

        if (ScoreDataManager.Instance == null)
        {
            Debug.LogError("[ScoreBoardManager] ScoreDataManager.Instance is NULL!");
            return;
        }

        List<ScoreDataManager.ScoreEntry> entries = type == "pedestrian"
            ? ScoreDataManager.Instance.GetPedestrianScoreboard()
            : ScoreDataManager.Instance.GetCyclistScoreboard();

        Debug.Log($"[ScoreBoardManager] Retrieved {entries.Count} scoreboard entries");

        for (int i = 0; i < scoreboardEntries.Length; i++)
        {
            if (scoreboardEntries[i] == null)
            {
                Debug.LogWarning($"[ScoreBoardManager] scoreboardEntries[{i}] is NULL!");
                continue;
            }

            if (i < entries.Count)
            {
                string displayID = entries[i].playerID;
                int score = entries[i].score;
                scoreboardEntries[i].text = $"{i + 1}. {displayID} - {score}";
                Debug.Log($"[ScoreBoardManager] Entry {i}: {scoreboardEntries[i].text}");
            }
            else
            {
                scoreboardEntries[i].text = $"{i + 1}. ---";
            }
        }

        Debug.Log("[ScoreBoardManager] ========== DisplayScoreboard completed ==========");
    }
}