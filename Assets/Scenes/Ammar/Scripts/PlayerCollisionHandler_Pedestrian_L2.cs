using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCollisionHandler_Pedestrian_L2 : MonoBehaviour
{
    public TextMeshProUGUI InstructionText;
    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public AudioSource warningSound;
    public PopUpWindow_Pedestrian popup;

    // Death Popup
    public Transform deathPopup;
    public Sprite deathSprite_HardPenalty;
    public Sprite deathSprite_SoftPenalty;
    public UnityEngine.UI.Image deathPopupImage;

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

            if (!warningShown)
            {
                warningShown = true;
                if (popup != null) popup.ShowWarning();
                if (warningSound != null) warningSound.Play();
            }

            if (rightLaneTimer >= rightLaneTimeLimit)
                TriggerGameOver("You spent too long on the right side!\nRemember to keep left.", 1);
        }
        else
        {
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
            TriggerGameOver("You hit an obstacle!\nStay on the pedestrian path.", 0);
            return;
        }

        if (collision.gameObject.CompareTag("npc"))
        {
            TriggerGameOver("You blocked another pedestrian!\nKeep left to let others pass.", 0);
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
            TriggerGameOver("You entered the cycling lane!\nThe red lane is for cyclists only.", 0);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("pedestrian_lane_right"))
            isInRightLane = false;
    }

    void TriggerGameOver(string reason, int popupType)
    {
        if (isGameOver) return;
        isGameOver = true;
        isInRightLane = false;

        if (backgroundMusic != null) backgroundMusic.Pause();
        if (gameOverSound != null) gameOverSound.Play();
        if (popup != null) popup.HideWarning();

        StartCoroutine(ShowDeathPopup(reason, popupType));
    }

    IEnumerator ShowDeathPopup(string reason, int popupType)
    {
        Time.timeScale = 0f;

        if (deathPopup != null)
        {
            deathPopup.GetComponentInChildren<TextMeshProUGUI>().text = reason + "\n\nLet's try that again!";
            if (deathPopupImage != null)
                deathPopupImage.sprite = popupType == 0 ? deathSprite_HardPenalty : deathSprite_SoftPenalty;
            deathPopup.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}