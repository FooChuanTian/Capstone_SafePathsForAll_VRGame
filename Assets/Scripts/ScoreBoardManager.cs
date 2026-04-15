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

    // --- Cached values set during DisplayScore, used by SaveScore ---
    private bool cachedIsPedestrian;
    private bool displayScoreRan = false;

    private int cached_livesLeft;
    private int cached_secondsCorrect;
    private int cached_secondsWrong;
    private int cached_totalScore;

    // Pedestrian only
    private int cached_phoneOpened;

    // Cyclist only
    private float cached_timeToSlow;
    private float cached_timeToStop;
    private int cached_speedPenalty;
    // ----------------------------------------------------------------

    void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (scoreManager == null || livesManager == null)
        {
            if (player != null)
            {
                scoreManager = player.GetComponent<ScoreManager>();
                livesManager = player.GetComponent<LivesManager>();

                // Also try children in case components are on a child object
                if (scoreManager == null)
                    scoreManager = player.GetComponentInChildren<ScoreManager>();
                if (livesManager == null)
                    livesManager = player.GetComponentInChildren<LivesManager>();

                if (scoreManager == null)
                    Debug.LogError("[ScoreBoardManager] ScoreManager not found on Player or its children!");
                else
                    Debug.Log($"[ScoreBoardManager] ScoreManager found - Instance ID: {scoreManager.GetInstanceID()}");

                if (livesManager == null)
                    Debug.LogError("[ScoreBoardManager] LivesManager not found on Player or its children!");
                else
                    Debug.Log($"[ScoreBoardManager] LivesManager found - Instance ID: {livesManager.GetInstanceID()}");
            }
            else
            {
                Debug.LogError("[ScoreBoardManager] Player GameObject is not assigned!");
            }
        }
    }

    void Update()
    {
        if (gameObject.activeSelf && livesManager != null && scoreManager != null)
            DisplayScore();
    }

    // Called by OpenScoreBoard when scoreboard becomes visible
    public void RefreshScoreboard(string type, bool isFinalSim)
    {
        scoreSaved = false;
        displayScoreRan = false;

        if (livesManager == null || scoreManager == null)
        {
            Debug.Log("[ScoreBoardManager] Components not initialized, calling InitializeComponents");
            InitializeComponents();
        }

        // DisplayScore reads from managers AND caches the values
        DisplayScore();

        // SaveScore uses cached values — guaranteed same data as what's displayed
        SaveScore();

        if (isFinalSim)
        {
            Debug.Log("[ScoreBoardManager] Final sim - displaying leaderboard");
            DisplayScoreboard(type);
        }
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

        // Cache shared values
        cached_livesLeft = livesLeft;
        cached_secondsCorrect = secondsCorrect;
        cached_secondsWrong = secondsWrong;

        if (LivesLeftText != null)
            LivesLeftText.text = livesLeft.ToString();
        if (CorrectLaneScoreText != null)
            CorrectLaneScoreText.text = secondsCorrect.ToString();
        if (WrongLaneScoreText != null)
            WrongLaneScoreText.text = secondsWrong.ToString();

        // Pedestrian mode
        if (PhoneOpenedText != null)
        {
            cachedIsPedestrian = true;

            int phoneOpened = scoreManager.PhoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);

            cached_phoneOpened = phoneOpened;
            cached_totalScore = totalScore;

            PhoneOpenedText.text = phoneOpened.ToString();
            if (TotalScoreText != null)
                TotalScoreText.text = totalScore.ToString();

            Debug.Log($"[ScoreBoardManager] DisplayScore (PEDESTRIAN) - Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}, Phone: {phoneOpened}, Total: {totalScore}");
        }
        // Cyclist mode
        else if (TimeSlowText != null && TimeStopText != null && SpeedPenaltyText != null)
        {
            cachedIsPedestrian = false;

            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10)
                             - (int)(timeToSlow / 0.25f * 2)
                             - (int)(timeToStop / 0.25f * 2)
                             - speedPenalty;

            cached_timeToSlow = timeToSlow;
            cached_timeToStop = timeToStop;
            cached_speedPenalty = speedPenalty;
            cached_totalScore = totalScore;

            TimeSlowText.text = timeToSlow.ToString("F2");
            TimeStopText.text = timeToStop.ToString("F2");
            SpeedPenaltyText.text = speedPenalty.ToString();
            if (TotalScoreText != null)
                TotalScoreText.text = totalScore.ToString();

            Debug.Log($"[ScoreBoardManager] DisplayScore (CYCLIST) - Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}, TimeSlow: {timeToSlow}, TimeStop: {timeToStop}, Speed: {speedPenalty}, Total: {totalScore}");
        }
        else
        {
            Debug.LogError("[ScoreBoardManager] DisplayScore: Could not determine player type!");
            Debug.LogError($"  PhoneOpenedText={PhoneOpenedText}, TimeSlowText={TimeSlowText}, TimeStopText={TimeStopText}, SpeedPenaltyText={SpeedPenaltyText}");
            return;
        }

        displayScoreRan = true;
    }

    void SaveScore()
    {
        Debug.Log("[ScoreBoardManager] SaveScore called");

        if (scoreSaved)
        {
            Debug.Log("[ScoreBoardManager] Score already saved, skipping");
            return;
        }

        if (!displayScoreRan)
        {
            Debug.LogError("[ScoreBoardManager] SaveScore called before DisplayScore ran successfully — aborting!");
            return;
        }

        if (ScoreDataManager.Instance == null)
        {
            Debug.LogError("[ScoreBoardManager] ScoreDataManager.Instance is NULL!");
            return;
        }

        // Use cached values — identical to what was displayed on screen
        if (cachedIsPedestrian)
        {
            bool isFirstSim = PedestrianGameNavigationManager.Instance == null;
            Debug.Log($"[ScoreBoardManager] Saving PEDESTRIAN - Total: {cached_totalScore}, Lives: {cached_livesLeft}, Correct: {cached_secondsCorrect}, Wrong: {cached_secondsWrong}, Phone: {cached_phoneOpened}, isFirst: {isFirstSim}");
            ScoreDataManager.Instance.SavePedestrianScore(cached_totalScore, cached_livesLeft, cached_secondsCorrect, cached_secondsWrong, cached_phoneOpened, isFirstSim);
        }
        else
        {
            bool isFirstSim = GameNavigationManager.Instance == null;
            Debug.Log($"[ScoreBoardManager] Saving CYCLIST - Total: {cached_totalScore}, Lives: {cached_livesLeft}, Correct: {cached_secondsCorrect}, Wrong: {cached_secondsWrong}, TimeSlow: {cached_timeToSlow}, TimeStop: {cached_timeToStop}, Speed: {cached_speedPenalty}, isFirst: {isFirstSim}");
            ScoreDataManager.Instance.SaveCyclistScore(cached_totalScore, cached_livesLeft, cached_secondsCorrect, cached_secondsWrong, cached_timeToSlow, cached_timeToStop, cached_speedPenalty, isFirstSim);
        }

        scoreSaved = true;
        Debug.Log("[ScoreBoardManager] Save complete!");
    }

    public void DisplayScoreboard(string type)
    {
        Debug.Log($"[ScoreBoardManager] DisplayScoreboard called with type: {type}");

        if (scoreboardEntries == null || scoreboardEntries.Length == 0)
        {
            Debug.LogError("[ScoreBoardManager] scoreboardEntries is null or empty!");
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
                scoreboardEntries[i].text = $"{i + 1}. {entries[i].playerID} - {entries[i].score}";
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