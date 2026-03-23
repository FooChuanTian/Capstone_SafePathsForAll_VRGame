using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI TotalScoreText;
    public TextMeshProUGUI WrongLaneScoreText;
    public TextMeshProUGUI CorrectLaneScoreText;
    public TextMeshProUGUI PhoneOpenedText;
    public TextMeshProUGUI SpeedControlText;
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
        int phoneOpened = scoreManager.PhoneOpened;

        WrongLaneScoreText.text = "" + secondsWrong;
        CorrectLaneScoreText.text = "" + secondsCorrect;
        LivesLeftText.text = "" + livesLeft;
        if (PhoneOpenedText)
        {
            // Score for pedestrian
            PhoneOpenedText.text = "" + phoneOpened;
            int totalScore = (livesLeft * 10) + secondsCorrect - (secondsWrong * 5);
            TotalScoreText.text = "" + totalScore;
        }
        else if (SpeedControlText)
        {
            // Score for cyclist
        }


    }
}
