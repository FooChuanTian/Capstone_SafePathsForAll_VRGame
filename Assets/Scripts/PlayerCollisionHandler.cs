using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Collections;
using Unity.VisualScripting;
using System.Data;

public class PlayerCollisionHandler : MonoBehaviour
{
    public TextMeshProUGUI WhichLaneText;
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;
    public Transform Checkpoint0;
    public Transform Player;
    public int warningCount = 0;
    public int maxWarnings = 200;
    public bool isGameOver = false;
    private bool isCyclingPath = false;
    private float timeToRespawn;
    private List<Transform> CheckpointList = new List<Transform>();
    void Start()
    {

    }
    void Update()
    {
        if (isGameOver)
        {
            timeToRespawn -= Time.deltaTime;
            if (timeToRespawn <= 0)
            {
                isGameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cyclist") || collision.gameObject.CompareTag("pedestrian"))
        {
            LivesManager livesManager = Player.gameObject.GetComponent<LivesManager>();
            Debug.Log("Collided");
            if (livesManager != null) {
                if (collision.gameObject.CompareTag("cyclist")) {
                    livesManager.NumLives -= 2;
                }
                else if (collision.gameObject.CompareTag("pedestrian"))
                {
                    collision.gameObject.GetComponent<Animator>().Play("Death");
                    livesManager.NumLives--;
                }
                livesManager.UpdateHearts(livesManager.NumLives);
                if (livesManager.NumLives <= 0) 
                {
                    PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
                    isGameOver = true;
                    timeToRespawn = 3f;
                    StartCoroutine(GameOver2("Lost all your lives"));
                    //positionManager.Teleport();
                }
            }
            
        }
        else if (collision.gameObject.CompareTag("checkpoint"))
        {
            Debug.Log("Checkpoint reached!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish") && !isGameOver)
        {
            InstructionText.text = "Success! You stayed on the left.";
            InstructionText.color = Color.green;

            Debug.Log("Scenario completed successfully!");

            isGameOver = true; // stops further logic
        }
    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("cycling_lane_left"))
        {
            isCyclingPath = true;
            WhichLaneText.text = "Cycling Lane Left";
            Debug.Log("On cycling path!");
        }
        else if (collision.gameObject.CompareTag("cycling_lane_right"))
        {
            isCyclingPath = true;
            WhichLaneText.text = "Cycling Lane Right";
            Debug.Log("On right cycling path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            isCyclingPath = false;
            WhichLaneText.text = "Pedestrian Lane Left";
            Debug.Log("On left pedestrian path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isCyclingPath = false;
            WhichLaneText.text = "Pedestrian Lane Right";
            Debug.Log("On right pedestrian path!");
            //PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            //isGameOver = true;
            //timeToRespawn = 3f;
            //StartCoroutine(GameOver2("You went onto the pedestrian lane!"));
        }
    }

    IEnumerator GameOver2(string reason)
    {
        Time.timeScale = 0;
        string outString = "Game over. \nReason: " + reason;
        GameOverText.text = outString;
        GameOverText.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameOverText.transform.parent.gameObject.SetActive(false);
    }
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over triggered.");

        // Get PlayerPositionManager
        PlayerPositionManager positionManager = Player.GetComponent<PlayerPositionManager>();

        if (positionManager != null)
        {
            positionManager.Teleport();
        }

        // Reset lesson state
        warningCount = 0;

        // Optional: delay before allowing new penalties
        Invoke(nameof(ResetGameState), 1.0f);
    }

    void ResetGameState()
    {
        isGameOver = false;
        InstructionText.text = "Try again. Stay left.";
        InstructionText.color = Color.white;
    }
}
