using TMPro;
using UnityEngine;

public class LaneLessonFeedback : MonoBehaviour
{
    public TextMeshProUGUI instructionText;
    public PlayerPositionManager positionManager;

    private bool isGameOver = false;

    void OnCollisionStay(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            instructionText.text = "Good! Stay left.";
            instructionText.color = Color.green;
        }

        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            instructionText.text = "Warning! Pedestrians must keep left.";
            instructionText.color = Color.yellow;
        }

        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            instructionText.text = "Game Over! You entered the cycling lane.";
            instructionText.color = Color.red;

            GameOver();
        }
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("Respawning player");

        if (positionManager != null)
        {
            positionManager.Teleport();
        }

        Invoke(nameof(ResetGame), 1f);
    }

    void ResetGame()
    {
        isGameOver = false;
        instructionText.text = "Try again. Stay left.";
        instructionText.color = Color.white;
    }
}