using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel; // The UI Panel background
    public TextMeshProUGUI tutorialText;
    public bool isTutorialActive = false;
    public int currentTutorialCheckpoint = 0; // Track which tutorial message to show
    public AudioSource backgroundMusic; // Reference to the background music AudioSource
    public AudioSource victorySoundEffect;
    public AudioSource checkpointSoundEffect;
    public GameObject popUpWindow; // Reference to the PopUpWindow script

    public void ShowTutorial(string message, int checkpointNumber)
    {   
        popUpWindow.SetActive(false); // Ensure any existing pop-ups are closed
        tutorialText.text = message;
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        currentTutorialCheckpoint = checkpointNumber;
        if (currentTutorialCheckpoint == 3)
        {
            backgroundMusic.Pause(); // Pause the background music when the tutorial is active
            victorySoundEffect.Play(); // Play the victory sound effect when the tutorial is shown
        } else
        {   if (currentTutorialCheckpoint !=1)
            {
                checkpointSoundEffect.Play(); // Play the checkpoint sound effect when the tutorial is shown
            }
            
        }
        
        // This freezes the physics and movement
        Time.timeScale = 0f; 
        
        // If using New Input System, you might need to disable player input here
    }

    public void CloseTutorial()
    {   
        tutorialPanel.SetActive(false);
        // popUpWindow.SetActive(true);
        
        // This resumes the game
        Time.timeScale = 1f; 
        isTutorialActive = false;
    }

    public void ClickToNextScene()
    {   
        if (currentTutorialCheckpoint == 1 || currentTutorialCheckpoint == 2) {
            CloseTutorial();
            PopUpWindow popupScript = FindObjectOfType<PopUpWindow>();
            popupScript.hasTriggered = false;
            return; // Just close the tutorial for checkpoint 1
        }
        Time.timeScale = 1f; // ALWAYS unfreeze before switching
        // SceneManager.LoadScene("Cyclist_lesson_endpage");
        GameNavigationManager.Instance.LoadNextDynamicScene();
    }
}