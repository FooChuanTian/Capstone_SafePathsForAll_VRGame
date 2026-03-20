using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PedestrianLessonCollision : MonoBehaviour
{
    public TextMeshProUGUI WhichLaneText;
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI GameOverText;

    public Transform Player;
    public Transform Checkpoint0;

    private bool isGameOver = false;

    void Start()
    {
        Player.position = Checkpoint0.position;
    }

    void OnCollisionStay(Collision collision)
    {
        if (isGameOver) return;

        // Correct lane
        if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            WhichLaneText.text = "Pedestrian Lane Left";
            InstructionText.text = "Good! Pedestrians should keep left.";
            InstructionText.color = Color.green;
        }

        // Wrong pedestrian side → warning
        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            WhichLaneText.text = "Pedestrian Lane Right";
            InstructionText.text = "Warning! Pedestrians should keep left.";
            InstructionText.color = Color.yellow;
        }

        // Cycling lane → game over
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            StartCoroutine(GameOver("You entered the cycling lane!"));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish") && !isGameOver)
        {
            InstructionText.text = "Success! You stayed on the left.";
            InstructionText.color = Color.green;

            Debug.Log("Lesson completed!");
        }
    }

    IEnumerator GameOver(string reason)
    {
        if (isGameOver) yield break;

        isGameOver = true;

        Time.timeScale = 0;

        GameOverText.text = "Game Over\nReason: " + reason;
        GameOverText.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3);

        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}