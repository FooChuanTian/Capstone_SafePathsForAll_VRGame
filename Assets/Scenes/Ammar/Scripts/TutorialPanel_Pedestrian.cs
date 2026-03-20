using UnityEngine;
using TMPro;

public class TutorialPanel_Pedestrian : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    public bool isTutorialActive = false;
    public int currentTutorialCheckpoint = 0;

    public AudioSource backgroundMusic;
    public AudioSource victorySoundEffect;
    public AudioSource checkpointSoundEffect;

    // ❌ REMOVE popup dependency completely
    // public GameObject popUpWindow;

    public void ShowTutorial(string message, int checkpointNumber)
    {
        tutorialText.text = message;
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        currentTutorialCheckpoint = checkpointNumber;

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
    }

    public void ClickToNextScene()
    {
        if (currentTutorialCheckpoint == 1 ||
            currentTutorialCheckpoint == 2 ||
            currentTutorialCheckpoint == 3)
        {
            CloseTutorial();
            return;
        }

        Time.timeScale = 1f;
        PedestrianGameNavigationManager.Instance.LoadNextScene();
    }
}