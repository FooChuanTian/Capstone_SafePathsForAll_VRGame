using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.XR;

public class FinishLine : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    public TutorialPanel tutorial_vr;
    private bool hasShownLaneTutorial = false;
    public Transform initialCheckpointLocation;
    private string currentsceneName;
    private int lessonIndex;

    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        lessonIndex = currentsceneName.Contains("lesson1") ? 1 : 
                      currentsceneName.Contains("lesson2") ? 2 : 
                      currentsceneName.Contains("lesson3") ? 3 : 0;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {   
            // cleanup();  // Disable for testing

            tutorial.ShowTutorial($"Congrats on completing lesson {lessonIndex}!", 4);
            if (XRSettings.enabled)
                tutorial_vr.ShowTutorial($"Congrats on completing lesson {lessonIndex}!", 4);
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
        } else if (collision.tag == "pedestrian" || collision.tag == "cyclist")
        {
            Destroy(collision.gameObject); // Remove the NPC from the scene
        }
    }

    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

}
