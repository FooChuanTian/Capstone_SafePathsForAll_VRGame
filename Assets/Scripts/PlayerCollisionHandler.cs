using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class PlayerCollisionHandler : MonoBehaviour
{
    private bool isCyclingPath = false;
    public TextMeshProUGUI InstructionText;
    public Transform Checkpoint0;
    private List<Transform> CheckpointList = new List<Transform>();
    public Transform Player;
    public int warningCount = 0;
    public int maxWarnings = 200;
    public bool isGameOver = false;


    void Start()
    {

        Player.transform.position = Checkpoint0.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cyclist"))
        {
            Debug.Log("Collided with cyclist");
            PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            positionManager.Teleport();
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
            InstructionText.text = "Cycling Lane Left";
            Debug.Log("On cycling path!");
        }
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            if (!isGameOver)
            {
                InstructionText.text = "Game Over! You entered the cycling lane.";
                InstructionText.color = Color.red;
                GameOver();
            }
        }

        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isCyclingPath = false;

            if (!isGameOver)
            {
                //warningCount++;
                InstructionText.text = "Warning! Pedestrians must keep left.";
                InstructionText.color = Color.yellow;
            }

            if (warningCount >= maxWarnings)
            {
                GameOver();
            }
        }

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
