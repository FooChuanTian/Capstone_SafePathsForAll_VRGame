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

    public void ShowTutorial(string message, int checkpointNumber)
    {
        tutorialText.text = message;
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        currentTutorialCheckpoint = checkpointNumber;
        
        // This freezes the physics and movement
        Time.timeScale = 0f; 
        
        // If using New Input System, you might need to disable player input here
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        
        // This resumes the game
        Time.timeScale = 1f; 
        isTutorialActive = false;
    }

    public void ClickToNextScene()
    {   
        if (currentTutorialCheckpoint == 1 || currentTutorialCheckpoint == 2) {
            CloseTutorial();
            return; // Just close the tutorial for checkpoint 1
        }
        Time.timeScale = 1f; // ALWAYS unfreeze before switching
        SceneManager.LoadScene("Cyclist_lesson1_endpage");
    }
}