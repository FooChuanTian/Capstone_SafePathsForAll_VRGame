using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI TotalScoreText;
    public TextMeshProUGUI WrongLaneScoreText;
    public TextMeshProUGUI CorrectLaneScoreText;
    private ScoreManager scoreManager;
    private LivesManager livesManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = transform.GetComponent<ScoreManager>();
        livesManager = transform.GetComponent<LivesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
