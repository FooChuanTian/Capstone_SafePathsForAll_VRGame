using TMPro;
using UnityEngine;
using System.Collections;
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
    private bool componentsInitialized = false;  // NEW: Prevent double initialization
    private int refreshCallCount = 0;  // DEBUG: Track how many times RefreshScoreboard is called

    void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // CRITICAL: If already initialized, DO NOT re-initialize!
        if (componentsInitialized)
        {
            Debug.Log("[ScoreBoardManager] Already initialized, SKIPPING to prevent overwrite!");
            return;
        }

        Debug.Log("[ScoreBoardManager] ===== InitializeComponents called =====");

        if (scoreManager == null || livesManager == null)
        {
            if (player != null)
            {
                Debug.Log($"[ScoreBoardManager] Player is assigned: {player.name}");
                Debug.Log($"[ScoreBoardManager] Player active in hierarchy: {player.activeInHierarchy}");

                scoreManager = player.GetComponent<ScoreManager>();
                livesManager = player.GetComponent<LivesManager>();

                if (scoreManager == null)
                {
                    Debug.LogError($"[ScoreBoardManager] ScoreManager NOT FOUND on {player.name}!");
                    Debug.LogError("[ScoreBoardManager] Make sure the Player GameObject has a ScoreManager component!");
                }
                else
                {
                    Debug.Log($"[ScoreBoardManager] ✓ ScoreManager found on {player.name}");
                }

                if (livesManager == null)
                {
                    Debug.LogError($"[ScoreBoardManager] LivesManager NOT FOUND on {player.name}!");
                    Debug.LogError("[ScoreBoardManager] Make sure the Player GameObject has a LivesManager component!");
                }
                else
                {
                    Debug.Log($"[ScoreBoardManager] ✓ LivesManager found on {player.name}");
                }

                // Mark as initialized if both components found
                if (scoreManager != null && livesManager != null)
                {
                    componentsInitialized = true;
                    Debug.Log("[ScoreBoardManager] ✓✓ Components successfully initialized and locked!");
                }
            }
            else
            {
                Debug.LogError("[ScoreBoardManager] Player GameObject is NOT ASSIGNED in Inspector!");
                Debug.LogError("[ScoreBoardManager] Please assign the Player field in ScoreBoardManager component");
            }
        }
        else
        {
            Debug.Log("[ScoreBoardManager] Components already exist, skipping initialization");
            componentsInitialized = true;
        }

        Debug.Log("[ScoreBoardManager] ===== InitializeComponents complete =====");
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
        refreshCallCount++;
        Debug.Log($"[ScoreBoardManager] ========== RefreshScoreboard called #{refreshCallCount}: type={type}, isFinalSim={isFinalSim} ==========");
        Debug.Log($"[ScoreBoardManager] Called from GameObject: {gameObject.name}, Parent: {(transform.parent != null ? transform.parent.name : "None")}");

        // CRITICAL FIX: Reset save flag so we can save this simulation
        scoreSaved = false;

        // Make sure components are initialized FIRST
        if (livesManager == null || scoreManager == null)
        {
            Debug.Log("[ScoreBoardManager] Components not initialized, calling InitializeComponents");
            InitializeComponents();

            // Check again after initialization
            if (livesManager == null)
                Debug.LogError("[ScoreBoardManager] livesManager STILL NULL after InitializeComponents!");
            else
                Debug.Log("[ScoreBoardManager] livesManager successfully initialized");

            if (scoreManager == null)
                Debug.LogError("[ScoreBoardManager] scoreManager STILL NULL after InitializeComponents!");
            else
                Debug.Log("[ScoreBoardManager] scoreManager successfully initialized");
        }
        else
        {
            Debug.Log("[ScoreBoardManager] Components already initialized, skipping InitializeComponents");
        }

        // Check if initialization succeeded
        if (livesManager == null || scoreManager == null)
        {
            Debug.LogError("[ScoreBoardManager] CRITICAL: Components still null after initialization!");
            Debug.LogError("[ScoreBoardManager] Cannot save score - exiting RefreshScoreboard");
            return;
        }

        Debug.Log("[ScoreBoardManager] Components are valid, proceeding with display and save");

        // IMPORTANT: Display score (will show zeros initially, Update() will fix it)
        DisplayScore();

        // CRITICAL FIX: Delay save by 0.1 seconds to let UI populate via Update()
        CancelInvoke(nameof(SaveScore));
        CancelInvoke(nameof(DelayedDisplayScoreboard));
        Invoke(nameof(SaveScore), 0.1f);
        if (isFinalSim)
        {
            Invoke(nameof(DelayedDisplayScoreboard), 0.2f);
            currentScoreboardType = type;
        }

        Debug.Log("[ScoreBoardManager] ========== RefreshScoreboard complete ==========");
    }

    private void DelayedDisplayScoreboard()
    {
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

        // NEW APPROACH: Read from UI text fields that DisplayScore() has populated
        // The UI shows the correct values, so we read directly from there!
        int livesLeft = 0;
        int secondsCorrect = 0;
        int secondsWrong = 0;

        // Parse values from UI text
        if (LivesLeftText != null && int.TryParse(LivesLeftText.text, out int lives))
            livesLeft = lives;
        else
            Debug.LogWarning("[ScoreBoardManager] Could not parse Lives from UI text");

        if (CorrectLaneScoreText != null && int.TryParse(CorrectLaneScoreText.text, out int correct))
            secondsCorrect = correct;
        else
            Debug.LogWarning("[ScoreBoardManager] Could not parse Correct from UI text");

        if (WrongLaneScoreText != null && int.TryParse(WrongLaneScoreText.text, out int wrong))
            secondsWrong = wrong;
        else
            Debug.LogWarning("[ScoreBoardManager] Could not parse Wrong from UI text");

        Debug.Log($"[ScoreBoardManager] VALUES READ FROM UI - Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}");

        // Pedestrian mode - check if PhoneOpened field exists
        if (PhoneOpenedText != null)
        {
            int phoneOpened = 0;
            if (int.TryParse(PhoneOpenedText.text, out int phone))
                phoneOpened = phone;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse Phone from UI text");

            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);
            Debug.Log($"[ScoreBoardManager] PEDESTRIAN - Phone: {phoneOpened}, Total: {totalScore}");

            bool isFirstSim = PedestrianGameNavigationManager.Instance == null;
            Debug.Log($"[ScoreBoardManager] Saving to CSV: totalScore={totalScore}, lives={livesLeft}, correct={secondsCorrect}, wrong={secondsWrong}, phone={phoneOpened}, isFirst={isFirstSim}");

            ScoreDataManager.Instance.SavePedestrianScore(totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened, isFirstSim);
        }
        // Cyclist mode - check if cyclist-specific fields exist
        else if (TimeSlowText != null && TimeStopText != null && SpeedPenaltyText != null)
        {
            float timeToSlow = 0;
            float timeToStop = 0;
            int speedPenalty = 0;

            if (float.TryParse(TimeSlowText.text, out float slow))
                timeToSlow = slow;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse TimeSlow from UI text");

            if (float.TryParse(TimeStopText.text, out float stop))
                timeToStop = stop;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse TimeStop from UI text");

            if (int.TryParse(SpeedPenaltyText.text, out int speed))
                speedPenalty = speed;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse SpeedPenalty from UI text");

            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int)(timeToSlow / 0.25f * 2) - (int)(timeToStop / 0.25f * 2) - speedPenalty;
            Debug.Log($"[ScoreBoardManager] CYCLIST - Total: {totalScore}, Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}, TimeSlow: {timeToSlow}, TimeStop: {timeToStop}, Speed: {speedPenalty}");

            bool isFirstSim = GameNavigationManager.Instance == null;
            Debug.Log($"[ScoreBoardManager] About to save cyclist - isFirstSim={isFirstSim}");

            ScoreDataManager.Instance.SaveCyclistScore(totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty, isFirstSim);
        }
        else
        {
            Debug.LogError("[ScoreBoardManager] Could not determine player type! Text fields are null.");
            Debug.LogError($"[ScoreBoardManager] PhoneOpenedText={PhoneOpenedText}, TimeSlowText={TimeSlowText}, TimeStopText={TimeStopText}, SpeedPenaltyText={SpeedPenaltyText}");
        }

        scoreSaved = true;
        Debug.Log("[ScoreBoardManager] ========== Save complete - values read from UI ==========");
    }

    void DisplayScore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (livesManager == null || scoreManager == null)
        {
            Debug.LogError("[ScoreBoardManager] livesManager or scoreManager is null in DisplayScore!");
            return;
        }

        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        // Display ONLY the numbers (labels are separate UI elements)
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
        Debug.Log($"[ScoreBoardManager] DisplayScoreboard called with type: {type}");

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

        Debug.Log($"[ScoreBoardManager] Found {entries.Count} entries to display");

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
                scoreboardEntries[i].text = $"{i + 1}. {displayID} - {entries[i].score}";
                Debug.Log($"[ScoreBoardManager] Set entry {i}: {scoreboardEntries[i].text}");
            }
            else
            {
                scoreboardEntries[i].text = $"{i + 1}. ---";
            }
        }

        Debug.Log("[ScoreBoardManager] DisplayScoreboard completed!");
    }
}