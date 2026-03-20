using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollisionHandler_Pedestrian : MonoBehaviour
{
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;

    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public PopUpWindow_Pedestrian popup;

    private bool isGameOver = false;
    private bool isOnPedestrian = false;
    private bool isOnCycling = false;

    void Update()
    {
        if (isGameOver) return;

        // Evaluate lane AFTER physics settles
        if (isOnPedestrian)
        {
            InstructionText.text = "Good! Stay on pedestrian path";
            InstructionText.color = Color.green;

            if (popup != null)
                popup.HideWarning();
        }
        else if (isOnCycling)
        {
            if (popup != null)
                popup.ShowWarning();

            TriggerGameOver("You entered the cycling lane!");
        }

        // reset for next frame
        isOnPedestrian = false;
        isOnCycling = false;
    }

    // ✅ LANE DETECTION (MATCH CYCLIST SYSTEM)
    void OnCollisionStay(Collision collision)
    {
        if (isGameOver) return;

        Debug.Log("COLLISION STAY: " + collision.gameObject.name + " | TAG: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("pedestrian_lane_left") ||
            collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isOnPedestrian = true;
        }
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            isOnCycling = true;
        }
    }

    // ✅ NPC / BIKE COLLISION
    void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("cyclist") ||
            collision.gameObject.CompareTag("obstacle"))
        {
            TriggerGameOver("You hit an obstacle!");
        }

        if (collision.gameObject.CompareTag("npc"))
        {
            TriggerGameOver("You blocked another pedestrian!");
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

        Time.timeScale = 0;

        GameOverText.text = "Game Over\n" + reason;
        GameOverText.transform.parent.gameObject.SetActive(true);

        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}