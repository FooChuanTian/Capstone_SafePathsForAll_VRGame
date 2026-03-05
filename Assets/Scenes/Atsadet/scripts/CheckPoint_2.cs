using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Checkpoint_2 : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPopup tutorial;
    private bool hasShownLaneTutorial = false;
    public PopUpWindow popUpWindow;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            // Todo 1: Show a message to the player
            // Todo 2: Play a sound effect
            // Todo 3: Wait for a few seconds before loading the next scene
            popUpWindow.ForceCloseAlert(); // Ensure any existing pop-ups are closed

            tutorial.ShowTutorial("Obstacle ahead! Avoid them but maintain left afterwards", 2);
            hasShownLaneTutorial = true; // Ensures it only freezes the game once

            // SceneManager.LoadScene("Cyclist_lesson1_endpage");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            // This allows us to set the build order scene in Unity editor and 
            // then use this same script to jump to the next scene in the sequence of the build scene order in build profiles

            // Update the manager on the player
            PlayerPositionManager positionManager = collision.GetComponent<PlayerPositionManager>();
            if (positionManager != null)
            {
                positionManager.UpdatePreviousPosition();
            }
        }
    }

}
