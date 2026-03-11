using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint_2 : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    private bool hasShownLaneTutorial = false;
    public Transform checkPointlocation_2;
    private string currentsceneName;
    private string displayMessage;
    public GameObject PedestrianObject;
    public Rigidbody rb;
    private GameObject Clone;
    private List<Vector3> spawnPoints = new List<Vector3>
    {
        new Vector3(0, 0, 35),
        new Vector3(-11, 0, 43),
        new Vector3(-15, 0, 50),
        new Vector3(-7, 0, 54),
        new Vector3(-9, 0, 64)
    };
    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        Debug.Log("TEST:Current scene: " + currentsceneName); // Debug log to check the current scene name
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            // Todo 1: Show a message to the player
            // Todo 2: Play a sound effect
            // Todo 3: Wait for a few seconds before loading the next scene
            // popUpWindow.ForceCloseAlert(); // Ensure any existing pop-ups are closed
            cleanup();

            if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
            {
                displayMessage = "Pedestrians ahead using the wrong lane! Ring them and maintain your lane.";
                runLesson1stage2();
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {
                displayMessage = "Obstacle ahead! Avoid them but maintain left afterwards";
            }
            else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            {
                displayMessage = "Crowded areas require slowing down. Watch out for those areas ahead.";
            }
            tutorial.ShowTutorial(displayMessage, 2);
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

    void runLesson1stage2()
    {   
        for (int i = spawnPoints.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints[i];
            Clone = Instantiate(PedestrianObject, spawnPoint, new Quaternion(0, -0.90f, 0, 1)) as GameObject;

            GameNavigationManager.Instance.activeClones.Add(Clone);

            // To make animation slightly diff for each otter
            Animator anim = Clone.GetComponent<Animator>();
            if (anim != null)
            {
                // Start at a random point in the walk cycle (0.0 to 1.0)
                float randomStart = UnityEngine.Random.Range(0f, 1f);
                
                // "Walk" should be the name of your state in the Animator window
                anim.Play("Walk", 0, randomStart); 
                
                // Optional: Randomize speed slightly so they don't stay in sync long-term
                anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            }
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            // int rnd_velo = UnityEngine.Random.Range(5, 9);
            float rnd_velo = UnityEngine.Random.Range(-0.5f, -1.0f);
            // float rnd_velo = -0.5f; // negative so it moves forward on the track away from checkpoint 2
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = Clone.gameObject;
            // Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
            // cloneRb.linearVelocity = new Vector3(rnd_velo * 5, 0, 0);
        }
    }
    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

}
// coordinates: 
// 0,0,35
// -11, 0, 43
// -15,0,50
// -7,0,54
// -9,0,64
