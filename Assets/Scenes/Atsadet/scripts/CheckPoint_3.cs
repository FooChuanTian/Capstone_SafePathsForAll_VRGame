using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint_3 : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    private bool hasShownLaneTutorial = false;
    public Transform checkPointlocation_2;
    private string currentsceneName;
    private string displayMessage;
    public GameObject CyclistObject;
    public Rigidbody rb;
    private GameObject Clone;
    float rnd_velo;
    private List<Vector3> spawnPoints = new List<Vector3>
    {
        new Vector3(-250, 0, 35),
        new Vector3(-300, 0, 43),
        new Vector3(-350, 0, 40),
        new Vector3(-380, 0, 60),
        new Vector3(-350, 0, 55)
    };
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
            cleanup();
            runLesson1stage3();
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

    void runLesson1stage3()
    {   
        for (int i = spawnPoints.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints[i];

            if (spawnPoint.z > 50)
            {
                Clone = Instantiate(CyclistObject, spawnPoint, new Quaternion(0, 180f, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(1f, 2f);
            } else
            {
                Clone = Instantiate(CyclistObject, spawnPoint, new Quaternion(0, 0, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(-0.5f, -1.0f);
            }
            

            GameNavigationManager.Instance.activeClones.Add(Clone);

            // To make animation slightly diff for each otter
            // Animator anim = Clone.GetComponent<Animator>();
            // if (anim != null)
            // {
            //     float randomStart = UnityEngine.Random.Range(0f, 1f);
            //     anim.Play("Walk", 0, randomStart); 
            //     anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            // }
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = Clone.gameObject;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
        }
    }

    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

}
