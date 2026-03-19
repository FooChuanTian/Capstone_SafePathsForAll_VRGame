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
    private List<Vector3> spawnPoints_lesson1 = new List<Vector3>
    {
        new Vector3(-250, 0, 35),
        new Vector3(-300, 0, 43),
        new Vector3(-350, 0, 40),
        new Vector3(-380, 0, 60),
        new Vector3(-350, 0, 55)
    };

    private List<Vector3> spawnPoints_lesson2 = new List<Vector3>
    {
        new Vector3(-400, 0, 60),
        new Vector3(-350, 0, 45),
        new Vector3(-350, 0, 65),
        new Vector3(-300, 0, 35),
        new Vector3(-320, 0, 55)
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
            cleanup(); //Disable for testing
            if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
            {
                displayMessage = "Slower cyclist ahead. When overtaking, remember to stay in the cyclist lane!";
                runLesson1Stage3();
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {   
                displayMessage = "Oncoming cyclist ahead using the wrong side! Ring to alert them and maintain your lane.";
                runLesson2Stage3();
            }
            else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            {
                displayMessage = "Follow the tactile strips!";
            }
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

    void runLesson1Stage3()
    {   
        for (int i = spawnPoints_lesson1.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints_lesson1[i];

            if (spawnPoint.z > 50)
            {
                Clone = Instantiate(CyclistObject, spawnPoint, new Quaternion(0, 180f, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(1f, 2f);
            } else
            {
                Clone = Instantiate(CyclistObject, spawnPoint, new Quaternion(0, 0, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(-0.5f, -1.0f);
            }
            

            GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

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

    void runLesson2Stage3()
    {   
        for (int i = spawnPoints_lesson2.Count - 1; i >= 0; i--)
        {   
            Vector3 spawnPoint = spawnPoints_lesson2[i];

            Clone = Instantiate(CyclistObject, spawnPoint, new Quaternion(0, 180f, 0, 1)) as GameObject;
            rnd_velo = UnityEngine.Random.Range(1f, 2f);

            GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

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
