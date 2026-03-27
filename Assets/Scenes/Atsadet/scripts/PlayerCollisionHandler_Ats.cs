using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Collections;
using Unity.VisualScripting;
using System.Data;
using UnityEngine.UI;


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
    private string currentsceneName;
    private int slowDownAreaSpeedLimit = 80;
    private int stopAreaSpeedLimit = 50;
    public Transform deathPopup;  // default sprite is type 0: Wrong lane
    public Image deathPopupImage;
    public Sprite deathSprite_HardPenalty;  //type 0 (Wrong lane/ Collision)
    public Sprite deathSprite_SoftPenalty;   //type 1 (Wrong side/ Speeding)
    public Transform recurringPopups; // AKA the warning popups for wrong side
    public AudioSource collisionSoundEffectSource;
    public AudioClip collisionSoundEffect;

    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name 
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
        if (collision.gameObject.CompareTag("cyclist") || collision.gameObject.CompareTag("pedestrian") || collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Collided");
            collisionSoundEffectSource.clip = collisionSoundEffect;
            collisionSoundEffectSource.Play();
            PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            isGameOver = true;
            timeToRespawn = 3f;
            lessonRoundWarningCount = 0; // reset warning count for next round
            // StartCoroutine(GameOver2("You crashed into " + collision.gameObject.name + "!"));
            StartCoroutine(GameOver2("You crashed into " + collision.gameObject.tag + "\nRemember to look out for other users or obstacles", 0));
        }
        else if (collision.gameObject.CompareTag("checkpoint"))
        {
            Debug.Log("Checkpoint reached!");
        }
        
    }

    void OnTriggerEnter(Collider other)
    {   
        CapsuleCollider myBody = GetComponent<CapsuleCollider>();
        Vector3 closestPoint = other.ClosestPoint(transform.position);

        if (other.CompareTag("finish") && !isGameOver)
        {
            InstructionText.text = "Success! You stayed on the left.";
            InstructionText.color = Color.green;

            Debug.Log("Scenario completed successfully!");

            isGameOver = true; // stops further logic
        } else if (other.CompareTag("speedtrackerStop") && !isGameOver && myBody.bounds.Contains(closestPoint))
        {
            Debug.Log("Speed tracker Stop reached!");
            if (Player.GetComponent<Rigidbody>().linearVelocity.magnitude > stopAreaSpeedLimit)
            {
                PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
                isGameOver = true;
                timeToRespawn = 3f;
                lessonRoundWarningCount = 0; // reset warning count for next round
                StartCoroutine(GameOver2("You were going too fast in the stop area\nRemember to slow down", 1));
            }

        } else if (other.CompareTag("speedtrackerSlow") && !isGameOver && myBody.bounds.Contains(closestPoint))
        {
            Debug.Log("Speed tracker Slow Down reached!");
            if (Player.GetComponent<Rigidbody>().linearVelocity.magnitude > slowDownAreaSpeedLimit)
            {
                PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
                isGameOver = true;
                timeToRespawn = 3f;
                lessonRoundWarningCount = 0; // reset warning count for next round
                StartCoroutine(GameOver2("You were going too fast in the slow area\nRemember to slow down", 1));
            }
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
            if (currentsceneName != "Cyclist_lesson1") // No warnings for right cycling lane in lesson 1 as it's not relevant to the learning outcomes of that lesson
            {
                lessonRoundWarningCount++;
                Debug.Log("TESTCOUNT: " + lessonRoundWarningCount);
                if (lessonRoundWarningCount >= lessonRoundMaxWarnings)
                {
                    PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
                    isGameOver = true;
                    timeToRespawn = 3f;
                    lessonRoundWarningCount = 0; // reset warning count for next round
                    StartCoroutine(GameOver2("You spent too long cycling on the wrong side\nRemember to stick to the left side of the cycling lane", 1));
                }
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
            StartCoroutine(GameOver2("You went to the wrong lane\nRemember to stick to the cyclist lane", 0));
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

    IEnumerator GameOver2(string reason, int popupType)
    {   
        backgroundMusicSource.Pause(); // Pause background music
        gameOverSoundEffectSource.Play(); // Play the game over sound effect
        Time.timeScale = 0;
        string outString = reason + "\n\nLet's try that again!";
        // GameOverText.text = outString;
        // GameOverText.transform.parent.gameObject.SetActive(true);

        // Hide the Recurring Popups
        recurringPopups.gameObject.SetActive(false);

        // Show different popups based on the type of game over
        deathPopup.GetComponentInChildren<TextMeshProUGUI>().text = outString;
        deathPopup.gameObject.SetActive(true);
        switch (popupType)
        {   case 0: //Hard penalty (Wrong lane/ Collision)
                deathPopupImage.sprite = deathSprite_HardPenalty;
                break;
            case 1: //Soft penalty (Wrong side/ Speeding)
                deathPopupImage.sprite = deathSprite_SoftPenalty;
                break;
            default:
                break;
        }

        yield return new WaitForSecondsRealtime(4);
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
