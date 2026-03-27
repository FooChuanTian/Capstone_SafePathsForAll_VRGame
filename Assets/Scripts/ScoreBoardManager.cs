using TMPro;
using UnityEngine;
using System.Collections.Generic;

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

    void Start()
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

                if (scoreManager == null)
                    Debug.LogError("[ScoreBoardManager] ScoreManager not found on Player!");
                if (livesManager == null)
                    Debug.LogError("[ScoreBoardManager] LivesManager not found on Player!");
            }
            else
            {
                Debug.LogError("[ScoreBoardManager] Player GameObject is not assigned!");
            }
        }
    }

    void Update()
    {
        if (gameObject.activeSelf)
            DisplayScore();
    }

    // Called by OpenScoreBoard when scoreboard becomes visible
    public void RefreshScoreboard(string type, bool isFinalSim)
    {
        // Save score first if not already saved
        SaveScore();

        // Update displayed stats
        DisplayScore();

        // Only show scoreboard on final simulation
        if (isFinalSim)
            DisplayScoreboard(type);
    }

    void SaveScore()
    {
        if (scoreSaved || ScoreDataManager.Instance == null) return;

        // Make sure components are initialized
        InitializeComponents();

        if (scoreManager == null || livesManager == null)
        {
            Debug.LogError("[ScoreBoardManager] Cannot save score - components not initialized!");
            return;
        }

        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        if (PhoneOpenedText)
        {
            int phoneOpened = scoreManager.PhoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);
            bool isFirstSim = PedestrianGameNavigationManager.Instance == null;
            ScoreDataManager.Instance.SavePedestrianScore(totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened, isFirstSim);
        }
        else if (TimeSlowText && TimeStopText && SpeedPenaltyText)
        {
            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int)(timeToSlow / 0.25f * 2) - (int)(timeToStop / 0.25f * 2) - speedPenalty;
            bool isFirstSim = GameNavigationManager.Instance == null;
            ScoreDataManager.Instance.SaveCyclistScore(totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty, isFirstSim);
        }

        scoreSaved = true;
    }

    void DisplayScore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Add null checks at the start
        if (livesManager == null || scoreManager == null)
        {
            UnityEngine.Debug.LogError("[ScoreBoardManager] livesManager or scoreManager is null in DisplayScore!");
            return;
        }

        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        // Add null checks for each text field
        if (WrongLaneScoreText != null)
            WrongLaneScoreText.text = "" + secondsWrong;
        else
            UnityEngine.Debug.LogError("[ScoreBoardManager] WrongLaneScoreText is NULL!");

        if (CorrectLaneScoreText != null)
            CorrectLaneScoreText.text = "" + secondsCorrect;
        else
            UnityEngine.Debug.LogError("[ScoreBoardManager] CorrectLaneScoreText is NULL!");

        if (LivesLeftText != null)
            LivesLeftText.text = "" + livesLeft;
        else
            UnityEngine.Debug.LogError("[ScoreBoardManager] LivesLeftText is NULL!");

        if (PhoneOpenedText)
        {
            int phoneOpened = scoreManager.PhoneOpened;
            PhoneOpenedText.text = "" + phoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);

            if (TotalScoreText != null)
                TotalScoreText.text = "" + totalScore;
            else
                UnityEngine.Debug.LogError("[ScoreBoardManager] TotalScoreText is NULL!");
        }
        else if (TimeSlowText && TimeStopText && SpeedPenaltyText)
        {
            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;
            TimeSlowText.text = "" + timeToSlow;
            TimeStopText.text = "" + timeToStop;
            SpeedPenaltyText.text = "" + speedPenalty;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int)(timeToSlow / 0.25f * 2) - (int)(timeToStop / 0.25f * 2) - speedPenalty;

            if (TotalScoreText != null)
                TotalScoreText.text = "" + totalScore;
            else
                UnityEngine.Debug.LogError("[ScoreBoardManager] TotalScoreText is NULL!");
        }
    }

    public void DisplayScoreboard(string type)
    {
        UnityEngine.Debug.Log($"[ScoreBoardManager] DisplayScoreboard called with type: {type}");

        if (scoreboardEntries == null)
        {
            UnityEngine.Debug.LogError("[ScoreBoardManager] scoreboardEntries is NULL!");
            return;
        }

        if (scoreboardEntries.Length == 0)
        {
            UnityEngine.Debug.LogError("[ScoreBoardManager] scoreboardEntries.Length is 0!");
            return;
        }

        if (ScoreDataManager.Instance == null)
        {
            UnityEngine.Debug.LogError("[ScoreBoardManager] ScoreDataManager.Instance is NULL!");
            return;
        }

        List<ScoreDataManager.ScoreEntry> entries = type == "pedestrian"
            ? ScoreDataManager.Instance.GetPedestrianScoreboard()
            : ScoreDataManager.Instance.GetCyclistScoreboard();

        UnityEngine.Debug.Log($"[ScoreBoardManager] Found {entries.Count} entries to display");

        for (int i = 0; i < scoreboardEntries.Length; i++)
        {
            if (scoreboardEntries[i] == null)
            {
                UnityEngine.Debug.LogWarning($"[ScoreBoardManager] scoreboardEntries[{i}] is NULL!");
                continue;
            }

            if (i < entries.Count)
            {
                // Get last 8 characters of player ID
                string shortID = entries[i].playerID.Length >= 8
                    ? entries[i].playerID.Substring(entries[i].playerID.Length - 8)
                    : entries[i].playerID;
                scoreboardEntries[i].text = $"{i + 1}. {shortID} - {entries[i].score}";
                UnityEngine.Debug.Log($"[ScoreBoardManager] Set entry {i}: {scoreboardEntries[i].text}");
            }
            else
            {
                scoreboardEntries[i].text = $"{i + 1}. ---";
            }
        }

        UnityEngine.Debug.Log("[ScoreBoardManager] DisplayScoreboard completed!");
    }
}