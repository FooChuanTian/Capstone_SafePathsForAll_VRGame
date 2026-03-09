using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Collections;
using Unity.VisualScripting;
using System.Data;

public class PlayerCollisionHandler_Ats : MonoBehaviour
{
    public TextMeshProUGUI WhichLaneText;
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;
    public Transform Checkpoint0;
    public Transform Player;
    public bool isGameOver = false;

    public Transform FilledHeart1;
    public Transform FilledHeart2;
    public Transform FilledHeart3;
    public Transform EmptyHeart1;
    public Transform EmptyHeart2;
    public Transform EmptyHeart3;
    private bool isCyclingPath = false;
    private float timeToRespawn;
    
    private List<Transform> CheckpointList = new List<Transform>();
    private int lifeCount = 3;
    private int lessonRoundWarningCount = 0;
    private int lessonRoundMaxWarnings = 300;
    public AudioSource gameOverSoundEffectSource;
    public AudioSource backgroundMusicSource;

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
            Debug.Log("Collided");
            // if (collision.gameObject.CompareTag("cyclist")) {
            //     lifeCount-= 2;
            // }
            // else if (collision.gameObject.CompareTag("pedestrian"))
            // {
            //     lifeCount--;
            // }
            // UpdateHearts(lifeCount);
            // if (lifeCount <= 0) 
            // {
            //     PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            //     isGameOver = true;
            //     timeToRespawn = 3f;
            //     StartCoroutine(GameOver2("Hit by cyclist"));
            //     //positionManager.Teleport();
            // }
        }
        else if (collision.gameObject.CompareTag("checkpoint"))
        {
            Debug.Log("Checkpoint reached!");
        }
        // else if (collision.gameObject.CompareTag("cycling_lane_right"))
        // {
        //     lessonRoundWarningCount++;
        //     Debug.Log("Warning count: " + lessonRoundWarningCount);
        // }
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
            lessonRoundWarningCount++;
            if (lessonRoundWarningCount >= lessonRoundMaxWarnings)
            {
                PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
                isGameOver = true;
                timeToRespawn = 3f;
                lessonRoundWarningCount = 0; // reset warning count for next round
                StartCoroutine(GameOver2("You spent too long on the wrong side!!"));
            }
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
            PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            isGameOver = true;
            timeToRespawn = 3f;
            lessonRoundWarningCount = 0; // reset warning count for next round
            StartCoroutine(GameOver2("You went onto the pedestrian lane!"));
        }
    }

    // void UpdateHearts(int livesLeft)
    // {
    //     switch (livesLeft)
    //     {
    //         case 0:
    //             FilledHeart1.gameObject.SetActive(false);
    //             FilledHeart2.gameObject.SetActive(false);
    //             FilledHeart3.gameObject.SetActive(false);
    //             EmptyHeart1.gameObject.SetActive(true);
    //             EmptyHeart2.gameObject.SetActive(true);
    //             EmptyHeart3.gameObject.SetActive(true);
    //             break;
    //         case 1:
    //             FilledHeart1.gameObject.SetActive(false);
    //             FilledHeart2.gameObject.SetActive(false);
    //             FilledHeart3.gameObject.SetActive(true);
    //             EmptyHeart1.gameObject.SetActive(true);
    //             EmptyHeart2.gameObject.SetActive(true);
    //             EmptyHeart3.gameObject.SetActive(false);
    //             break;
    //         case 2:
    //             FilledHeart1.gameObject.SetActive(false);
    //             FilledHeart2.gameObject.SetActive(true);
    //             FilledHeart3.gameObject.SetActive(true);
    //             EmptyHeart1.gameObject.SetActive(true);
    //             EmptyHeart2.gameObject.SetActive(false);
    //             EmptyHeart3.gameObject.SetActive(false);
    //             break;
    //         case 3:
    //             FilledHeart1.gameObject.SetActive(true);
    //             FilledHeart2.gameObject.SetActive(true);
    //             FilledHeart3.gameObject.SetActive(true);
    //             EmptyHeart1.gameObject.SetActive(false);
    //             EmptyHeart2.gameObject.SetActive(false);
    //             EmptyHeart3.gameObject.SetActive(false);
    //             break;
    //     }
    // }

    IEnumerator GameOver2(string reason)
    {   
        backgroundMusicSource.Pause(); // Pause background music
        gameOverSoundEffectSource.Play(); // Play the game over sound effect
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
        // warningCount = 0;

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
