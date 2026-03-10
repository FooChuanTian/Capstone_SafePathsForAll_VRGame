using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Checkpoint_3 : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    private bool hasShownLaneTutorial = false;
    public Transform checkPointlocation_2;
    private string currentsceneName;
    private string displayMessage;
    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        Debug.Log("TEST:Current scene: " + currentsceneName); // Debug log to check the current scene name

        if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
        {
            displayMessage = "Slower cyclist ahead. When overtaking, remember to stay in the cyclist lane!";
        }
        else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
        {
            displayMessage = "Oncoming cyclist ahead using the wrong side! Ring to alert them and maintain your lane.";
        }
        else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
        {
            displayMessage = "Follow the tactile strips!";
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            // Todo 1: Show a message to the player
            // Todo 2: Play a sound effect
            // Todo 3: Wait for a few seconds before loading the next scene
            // popUpWindow.ForceCloseAlert(); // Ensure any existing pop-ups are closed

            tutorial.ShowTutorial(displayMessage, 3);
            hasShownLaneTutorial = true; // Ensures it only freezes the game once

            // SceneManager.LoadScene("Cyclist_lesson1_endpage");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            // This allows us to set the build order scene in Unity editor and 
            // then use this same script to jump to the next scene in the sequence of the build scene order in build profiles

            // Update the manager on the player
            PlayerPositionManager_Ats positionManager = collision.GetComponent<PlayerPositionManager_Ats>();
            if (positionManager != null)
            {
                positionManager.UpdatePreviousPosition(checkPointlocation_2);
            }
        }
    }

}
