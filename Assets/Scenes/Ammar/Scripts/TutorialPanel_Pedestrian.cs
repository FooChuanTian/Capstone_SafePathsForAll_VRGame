using UnityEngine;
using TMPro;

public class TutorialPanel_Pedestrian : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public System.Action onTutorialClosed;

    public bool isTutorialActive = false;
    public int currentTutorialCheckpoint = 0;

    public AudioSource backgroundMusic;
    public AudioSource victorySoundEffect;
    public AudioSource checkpointSoundEffect;

    public void ShowTutorial(string message, int checkpointNumber)
    {
        tutorialText.text = message;
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        currentTutorialCheckpoint = checkpointNumber;

        // 🔥 FIX 1: Unlock cursor so UI can be clicked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (currentTutorialCheckpoint == 4)
        {
            if (backgroundMusic != null)
                backgroundMusic.Pause();

            if (victorySoundEffect != null)
                victorySoundEffect.Play();
        }
        else
        {
            if (currentTutorialCheckpoint != 1 && checkpointSoundEffect != null)
                checkpointSoundEffect.Play();
        }

        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        isTutorialActive = false;

        // 🔥 FIX 2: Lock cursor back to gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Trigger callback AFTER unpausing
        if (onTutorialClosed != null)
        {
            onTutorialClosed.Invoke();
            onTutorialClosed = null;
        }
    }

    public void ClickToNextScene()
    {
        // FIX 3: Always restore time + cursor before switching scene
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (currentTutorialCheckpoint == 1 ||
            currentTutorialCheckpoint == 2 ||
            currentTutorialCheckpoint == 3)
        {
            CloseTutorial();
            return;
        }

        // Finish case (checkpoint 4)
        PedestrianGameNavigationManager.Instance.LoadNextScene();
    }
}