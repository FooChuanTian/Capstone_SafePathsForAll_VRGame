using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint_1 : MonoBehaviour
{   
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    private bool hasShownLaneTutorial = false;
    private string currentsceneName;
    private string displayMessage;
    public GameObject PedestrianObject;
    public Rigidbody rb;
    private GameObject Clone;
    private List<Vector3> spawnPoints = new List<Vector3>
    {
        new Vector3(488, 0, 2),
        new Vector3(450, 0, 5),
        new Vector3(300, 0, 17),
        new Vector3(270, 0, 13),
        new Vector3(270, 0, 17)
    };
    float rnd_velo;

    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        Debug.Log("TEST:Current scene: " + currentsceneName); // Debug log to check the current scene name
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
            {
                displayMessage = "Pedestrian lane is empty, but doesn't mean you should go there.";
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {
                displayMessage = "Right side is empty, but always keep left to practice good cycling habits!";
                runLesson2Stage1();
            }
            else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            {
                displayMessage = "Follow the tactile strips as per colour.";
            }

            tutorial.ShowTutorial(displayMessage, 1);
            hasShownLaneTutorial = true; // Ensures it only freezes the game once

            // SceneManager.LoadScene("Cyclist_lesson1_endpage");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            // This allows us to set the build order scene in Unity editor and 
            // then use this same script to jump to the next scene in the sequence of the build scene order in build profiles
        }
    }

    void runLesson2Stage1()
    {   
        for (int i = spawnPoints.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints[i];

            if (spawnPoint.z > 10)
            {
                Clone = Instantiate(PedestrianObject, spawnPoint, new Quaternion(0, 0.90f, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(1f, 4f);
            } else
            {
                Clone = Instantiate(PedestrianObject, spawnPoint, new Quaternion(0, -0.90f, 0, 1)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(-1f, -4f);
            }

            GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

            // To make animation slightly diff for each otter
            Animator anim = Clone.GetComponent<Animator>();
            if (anim != null)
            {
                float randomStart = UnityEngine.Random.Range(0f, 1f);

                anim.Play("Walk", 0, randomStart); // "Walk" is the name of default state in the Animator
                
                // Optional: Randomize speed slightly so they don't stay in sync long-term
                anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            }

            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = Clone.gameObject;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;

        }
    }

}
