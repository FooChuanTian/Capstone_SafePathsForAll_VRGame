using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class FinishLine : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    private bool hasShownLaneTutorial = false;
    public Transform initialCheckpointLocation;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {   
            cleanup();  // Disable for testing

            tutorial.ShowTutorial("Congrats on completing lesson 1!", 4);
            hasShownLaneTutorial = true; // Ensures it only freezes the game once

            // SceneManager.LoadScene("Cyclist_lesson1_endpage");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            // This allows us to set the build order scene in Unity editor and 
            // then use this same script to jump to the next scene in the sequence of the build scene order in build profiles
            PlayerPositionManager_Ats positionManager = collision.GetComponent<PlayerPositionManager_Ats>();
            if (positionManager != null)
            {
                positionManager.ChangePreviousCheckpoint(initialCheckpointLocation);
                positionManager.ResetPositionChangeFlag(); // Reset the flag 
            }
        }
    }

    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

}
