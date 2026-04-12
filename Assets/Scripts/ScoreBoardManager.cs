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

    void Awake()  // Changed from Start to Awake for earlier initialization
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
        // Only update display when scoreboard is active and components are valid
        if (gameObject.activeSelf && livesManager != null && scoreManager != null)
            DisplayScore();
    }

    // Called by OpenScoreBoard when scoreboard becomes visible
    public void RefreshScoreboard(string type, bool isFinalSim)
    {
        // Make sure components are initialized
        if (livesManager == null || scoreManager == null)
        {
            Debug.Log("[ScoreBoardManager] Components not initialized, calling InitializeComponents");
            InitializeComponents();
        }

        // IMPORTANT: Display score FIRST (this reads fresh values)
        DisplayScore();
        
        // THEN save (after values have been read and displayed)
        SaveScore();

        // Only show leaderboard on final simulation
        if (isFinalSim)
        {
            Debug.Log("[ScoreBoardManager] Final sim - displaying leaderboard");
            DisplayScoreboard(type);
        }
    }
    void SaveScore()
        {
            Debug.Log("[ScoreBoardManager] SaveScore called");
            
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

            // NEW APPROACH: Read values directly from the UI text fields that are already displaying correctly!
            // This guarantees we save exactly what the user sees on screen
            
            int livesLeft = 0;
            int secondsCorrect = 0;
            int secondsWrong = 0;
            
            // Parse values from the displayed text fields
            if (LivesLeftText != null && int.TryParse(LivesLeftText.text, out int lives))
                livesLeft = lives;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse LivesLeftText");

            if (CorrectLaneScoreText != null && int.TryParse(CorrectLaneScoreText.text, out int correct))
                secondsCorrect = correct;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse CorrectLaneScoreText");

            if (WrongLaneScoreText != null && int.TryParse(WrongLaneScoreText.text, out int wrong))
                secondsWrong = wrong;
            else
                Debug.LogWarning("[ScoreBoardManager] Could not parse WrongLaneScoreText");
                
            Debug.Log($"[ScoreBoardManager] VALUES FROM UI - Lives: {livesLeft}, Correct: {secondsCorrect}, Wrong: {secondsWrong}");

            // Pedestrian mode
            if (PhoneOpenedText != null && TotalScoreText != null)
            {
                int phoneOpened = 0;
                int totalScore = 0;
                
                if (int.TryParse(PhoneOpenedText.text, out int phone))
                    phoneOpened = phone;
                else
                    Debug.LogWarning("[ScoreBoardManager] Could not parse PhoneOpenedText");

                if (int.TryParse(TotalScoreText.text, out int score))
                    totalScore = score;
                else
                    Debug.LogWarning("[ScoreBoardManager] Could not parse TotalScoreText");
                
                Debug.Log($"[ScoreBoardManager] PEDESTRIAN FROM UI - Phone: {phoneOpened}, Total: {totalScore}");
                
                bool isFirstSim = PedestrianGameNavigationManager.Instance == null;
                
                Debug.Log($"[ScoreBoardManager] Saving to CSV: totalScore={totalScore}, lives={livesLeft}, correct={secondsCorrect}, wrong={secondsWrong}, phone={phoneOpened}, isFirst={isFirstSim}");
                
                ScoreDataManager.Instance.SavePedestrianScore(totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened, isFirstSim);
            }
            // Cyclist mode
            else if (TimeSlowText != null && TimeStopText != null && SpeedPenaltyText != null && TotalScoreText != null)
            {
                float timeToSlow = 0f;
                float timeToStop = 0f;
                int speedPenalty = 0;
                int totalScore = 0;
                
                if (float.TryParse(TimeSlowText.text, out float slow))
                    timeToSlow = slow;
                    
                if (float.TryParse(TimeStopText.text, out float stop))
                    timeToStop = stop;
                    
                if (int.TryParse(SpeedPenaltyText.text, out int penalty))
                    speedPenalty = penalty;
                    
                if (int.TryParse(TotalScoreText.text, out int score))
                    totalScore = score;
                
                Debug.Log($"[ScoreBoardManager] CYCLIST FROM UI - Total: {totalScore}");
                
                bool isFirstSim = GameNavigationManager.Instance == null;
                
                ScoreDataManager.Instance.SaveCyclistScore(totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty, isFirstSim);
            }
            else
            {
                Debug.LogError("[ScoreBoardManager] Could not determine player type! Text fields are null.");
            }
    scoreSaved = true;
            Debug.Log("[ScoreBoardManager] Save complete");
        }

    void DisplayScore()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Add null checks at the start
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
                // New IDs are already short (P_0001, C_0001), display them directly
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