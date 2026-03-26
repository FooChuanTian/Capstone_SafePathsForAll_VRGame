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
        scoreManager = player.GetComponent<ScoreManager>();
        livesManager = player.GetComponent<LivesManager>();
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

        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        WrongLaneScoreText.text = "" + secondsWrong;
        CorrectLaneScoreText.text = "" + secondsCorrect;
        LivesLeftText.text = "" + livesLeft;

        if (PhoneOpenedText)
        {
            int phoneOpened = scoreManager.PhoneOpened;
            PhoneOpenedText.text = "" + phoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);
            TotalScoreText.text = "" + totalScore;
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
            TotalScoreText.text = "" + totalScore;
        }
    }

    public void DisplayScoreboard(string type)
    {
        if (scoreboardEntries == null || scoreboardEntries.Length == 0 || ScoreDataManager.Instance == null) return;

        List<ScoreDataManager.ScoreEntry> entries = type == "pedestrian"
            ? ScoreDataManager.Instance.GetPedestrianScoreboard()
            : ScoreDataManager.Instance.GetCyclistScoreboard();

        for (int i = 0; i < scoreboardEntries.Length; i++)
        {
            if (scoreboardEntries[i] == null) continue;

            if (i < entries.Count)
                scoreboardEntries[i].text = $"{i + 1}. {entries[i].playerID} - {entries[i].score} ({entries[i].simRun})";
            else
                scoreboardEntries[i].text = $"{i + 1}. ---";
        }
    }
}