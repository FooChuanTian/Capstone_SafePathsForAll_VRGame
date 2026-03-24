using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler_Pedestrian_L2 : MonoBehaviour
{
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;

    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public AudioSource warningSound;
    public PopUpWindow_Pedestrian popup;

    // How long player can stay in right lane before game over
    public float rightLaneTimeLimit = 3f;

    private bool isGameOver = false;
    private float rightLaneTimer = 0f;
    private bool isInRightLane = false;
    private bool warningShown = false;

    void Update()
    {
        if (isGameOver) return;

        if (isInRightLane)
        {
            rightLaneTimer += Time.deltaTime;

            // Show warning at half the time limit
            if (rightLaneTimer >= rightLaneTimeLimit * 0.5f && !warningShown)
            {
                warningShown = true;
                if (popup != null) popup.ShowWarning();
                if (warningSound != null) warningSound.Play();
                Debug.Log("Right lane warning shown");
            }

            // Game over after time limit
            if (rightLaneTimer >= rightLaneTimeLimit)
            {
                TriggerGameOver("You spent too long on the right side! Keep left.");
            }
        }
        else
        {
            // Reset timer and warning when back on left lane
            if (rightLaneTimer > 0f)
            {
                rightLaneTimer = 0f;
                warningShown = false;
                if (popup != null) popup.HideWarning();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("cyclist") ||
            collision.gameObject.CompareTag("obstacle"))
        {
            TriggerGameOver("You hit an obstacle!");
            return;
        }

        if (collision.gameObject.CompareTag("npc"))
        {
            TriggerGameOver("You blocked another pedestrian!");
            return;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            isInRightLane = false;

            if (InstructionText != null)
            {
                InstructionText.text = "Good! Keep to the left.";
                InstructionText.color = Color.green;
            }
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isInRightLane = true;

            if (InstructionText != null)
            {
                InstructionText.text = "Move to the left side!";
                InstructionText.color = Color.yellow;
            }
        }
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            TriggerGameOver("You entered the cycling lane!");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isInRightLane = false;
        }
    }

    void TriggerGameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        isInRightLane = false;

        Debug.Log("GAME OVER: " + reason);

        if (backgroundMusic != null) backgroundMusic.Pause();
        if (gameOverSound != null) gameOverSound.Play();
        if (popup != null) popup.HideWarning();

        if (GameOverText != null)
        {
            GameOverText.text = "Game Over\n" + reason;
            GameOverText.transform.parent.gameObject.SetActive(true);
        }

        StartCoroutine(RestartSceneAfterDelay());
    }

    IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}