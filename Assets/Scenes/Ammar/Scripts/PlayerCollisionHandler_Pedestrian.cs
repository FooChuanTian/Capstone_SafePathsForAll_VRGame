using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler_Pedestrian : MonoBehaviour
{
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;

    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public PopUpWindow_Pedestrian popup;

    private bool isGameOver = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        Debug.Log("COLLISION: " + collision.gameObject.name + " | TAG: " + collision.gameObject.tag);

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

        if (collision.gameObject.CompareTag("pedestrian_lane_left") ||
            collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            if (InstructionText != null)
            {
                InstructionText.text = "Good! Stay on pedestrian path";
                InstructionText.color = Color.green;
            }

            if (popup != null)
                popup.HideWarning();
        }
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            if (popup != null)
                popup.ShowWarning();

            TriggerGameOver("You entered the cycling lane!");
        }
    }

    void TriggerGameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER: " + reason);

        if (backgroundMusic != null)
            backgroundMusic.Pause();

        if (gameOverSound != null)
            gameOverSound.Play();

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