using TMPro;
using UnityEngine;

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
    private ScoreManager scoreManager;
    private LivesManager livesManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = player.GetComponent<ScoreManager>();
        livesManager = player.GetComponent<LivesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            DisplayScore();
        }
    }

    void DisplayScore()
    {
        int livesLeft = livesManager.NumLives;
        int secondsCorrect = scoreManager.SecondsOnCorrectLane;
        int secondsWrong = scoreManager.SecondsOnWrongLane;

        WrongLaneScoreText.text = "" + secondsWrong;
        CorrectLaneScoreText.text = "" + secondsCorrect;
        LivesLeftText.text = "" + livesLeft;
        if (PhoneOpenedText)
        {
            // Score for pedestrian
            int phoneOpened = scoreManager.PhoneOpened;
            PhoneOpenedText.text = "" + phoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);
            TotalScoreText.text = "" + totalScore;
        }
        else if (TimeSlowText && TimeStopText && SpeedPenaltyText)
        {
            // Score for cyclist
            float timeToSlow = scoreManager.TimetoSlow;
            float timeToStop = scoreManager.TimetoStop;
            int speedPenalty = scoreManager.MaximumSpeedPenalty;
            TimeSlowText.text = "" + timeToSlow;
            TimeStopText.text = "" + timeToStop;
            SpeedPenaltyText.text = "" + speedPenalty;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 10) - (int) (timeToSlow / 0.25f * 2) - (int) (timeToStop / 0.25f * 2) - speedPenalty;
            TotalScoreText.text = "" + totalScore;
        }


    }
}
