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
    public TextMeshProUGUI GoalText;
    public GameObject pedestrianNPCObject;

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
                displayMessage = "Pedestrian lane may be empty but as a cyclist you should stick to the cyclist lane";
                GoalText.text = "Keep to the cyclist lane";
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {
                displayMessage = "The right side of the cyclist lane may be empty but always keep left to practice good cycling habits";
                GoalText.text = "Keep to the left side of the cyclist lane";
                runLesson2Stage1();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint1());
            }
            else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            {
                displayMessage = "Look out for our speed-guide tactile strips ahead!\nFollow them to practice navigating crowded areas safely\nYellow means slow down and red means stop";
                GoalText.text = "Follow the tactile strips to navigate crowded areas safely";
                runLesson3Stage1();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint1());
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
        int counter = 0;
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

            // GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

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

        while (counter < 3)
        {
            SpawnNPC_checkpoint1(167, 480, -7, 7);
            SpawnNPC_checkpoint1(167, 480, 13, 27);
            counter++;
        }
    }

    void runLesson3Stage1()
    {
        int counter = 0;
        while (counter < 3)
        {
            SpawnNPC_checkpoint1(167, 480, -7, 7);
            SpawnNPC_checkpoint1(167, 480, 13, 27);
            counter++;
        }
    }

    IEnumerator spawnRandomNPCsRoutine_checkpoint1()
    {   
        int counter = 0;
        while (counter < 3)
        {
            // Spawn at random intervals between 1 and 3 seconds
            float randomTimer = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(randomTimer);
            SpawnNPC_checkpoint1(167, 480, -7, 7);
            SpawnNPC_checkpoint1(167, 480, 13, 27);
            counter++;
        }
    }

    void SpawnNPC_checkpoint1(float minX, float maxX, float minZ, float maxZ)
    {   
        float random_z = Random.Range(minZ, maxZ);
        float rnd_velo = 0f;
        Vector3 randomPos = new Vector3(
            Random.Range(minX, maxX), 
            1, 
            random_z
        );
        if (random_z < 10)
        {
            Clone = Instantiate(pedestrianNPCObject, randomPos, new Quaternion(0, -0.90f, 0, 1)) as GameObject;
            rnd_velo = UnityEngine.Random.Range(-1f, -5f);
        } else
        {
            Clone = Instantiate(pedestrianNPCObject, randomPos, new Quaternion(0, 0.90f, 0, 1)) as GameObject;
            rnd_velo = UnityEngine.Random.Range(1f, 5f);

        }

        // GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

        // To make animation slightly diff for each otter
        Animator anim = Clone.GetComponent<Animator>();
        if (anim != null)
        {
            float randomStart = UnityEngine.Random.Range(3f, 5f);
            anim.Play("Walk", 0, randomStart); 
            anim.speed = UnityEngine.Random.Range(1f, 1.5f);
        }
        Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
        Clone.AddComponent<NPCStraight_Ats>();
        Clone.GetComponent<NPCStraight_Ats>().gb = Clone.gameObject;
        Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
    }

}
