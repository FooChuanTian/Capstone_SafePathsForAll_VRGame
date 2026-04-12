using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCollisionHandler_Pedestrian : MonoBehaviour
{
    public TextMeshProUGUI InstructionText;
    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public PopUpWindow_Pedestrian popup;

    // Death Popup
    public Transform deathPopup;
    public Sprite deathSprite_HardPenalty;
    public Sprite deathSprite_SoftPenalty;
    public UnityEngine.UI.Image deathPopupImage;

    private bool isGameOver = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("cyclist") ||
            collision.gameObject.CompareTag("obstacle"))
        {
            TriggerGameOver("You hit an obstacle!\nRemember to stay on the pedestrian path.", 0);
            return;
        }

        if (collision.gameObject.CompareTag("npc"))
        {
            TriggerGameOver("You walked into another pedestrian!\nBe aware of others around you.", 0);
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
        }
        else if (collision.gameObject.CompareTag("cycling_lane_left") ||
                 collision.gameObject.CompareTag("cycling_lane_right"))
        {
            if (popup != null) popup.ShowWarning();
            TriggerGameOver("You entered the cycling lane!\nThe red lane is for cyclists only.", 0);
        }
    }

    void TriggerGameOver(string reason, int popupType)
    {
        if (isGameOver) return;
        isGameOver = true;

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