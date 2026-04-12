using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;

public class Checkpoint_3 : MonoBehaviour
{   
    public Transform trackingSpaceObj;
    // public TextMeshProUGUI GoalText;
    public TutorialPanel tutorial;
    public TutorialPanel tutorial_vr;
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
        new Vector3(-250, 3, 35),
        new Vector3(-300, 3, 43),
        new Vector3(-350, 3, 40),
        new Vector3(-380, 3, 60),
        new Vector3(-350, 3, 55)
    };

    private List<Vector3> spawnPoints_lesson2 = new List<Vector3>
    {
        new Vector3(-400, 3, 60),
        new Vector3(-350, 3, 45),
        new Vector3(-350, 3, 65),
        new Vector3(-300, 3, 35),
        new Vector3(-320, 3, 55)
    };
    public GameObject pedestrianNPCObject;
    public TextMeshProUGUI GoalText;

    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        Debug.Log("TEST:Current scene: " + currentsceneName); // Debug log to check the current scene name
        trackingSpaceObj.localPosition = new Vector3(0, 10f, 0);   //THIS LINE
        trackingSpaceObj.localRotation = Quaternion.Euler(0, -90, 0);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            // cleanup(); //Disable for testing
            if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
            {
                displayMessage = "Watch out for slower cyclist ahead!\nWhen overtaking remember to stay in the cyclist lane";
                GoalText.text = "Keep to the cyclist lane";
                runLesson1Stage3();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint3());
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {   
                displayMessage = "Oncoming cyclist ahead using the wrong side!\nRing them and continue on the left side of thecyclist lane";
                GoalText.text = "Keep to the left side of the cyclist lane";
                runLesson2Stage3();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint3());
            }
            // else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            // {
            //     displayMessage = "Follow the tactile strips!";
            // }
            tutorial.ShowTutorial(displayMessage, 3);
            if (XRSettings.enabled) 
                tutorial_vr.ShowTutorial(displayMessage, 3);
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
        int counter = 0;
        for (int i = spawnPoints_lesson1.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints_lesson1[i];

            if (spawnPoint.z > 50)
            {
                Clone = Instantiate(CyclistObject, spawnPoint, Quaternion.Euler(0, 90, 0)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(1f, 2f);
            } else
            {
                Clone = Instantiate(CyclistObject, spawnPoint, Quaternion.Euler(0, -90, 0)) as GameObject;
                rnd_velo = UnityEngine.Random.Range(-0.5f, -1.0f);
            }
            

            // GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

            // To make animation slightly diff for each otter
            // Animator anim = Clone.GetComponent<Animator>();
            // if (anim != null)
            // {
            //     float randomStart = UnityEngine.Random.Range(0f, 1f);
            //     anim.Play("Walk", 0, randomStart); 
            //     anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            // }
            Rigidbody cloneRb = Clone.GetComponentInChildren<Rigidbody>();
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = cloneRb != null ? cloneRb.gameObject : Clone.gameObject;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
        }

        while (counter < 2)
        {
            SpawnNPC_checkpoint3(-500, -166, -7, 7);
            SpawnNPC_checkpoint3(-500, -166, 13, 27);
            counter++;
        }
    }

    void runLesson2Stage3()
    {   
        int counter = 0;
        for (int i = spawnPoints_lesson2.Count - 1; i >= 0; i--)
        {   
            Vector3 spawnPoint = spawnPoints_lesson2[i];

            Clone = Instantiate(CyclistObject, spawnPoint, Quaternion.Euler(0, 90, 0)) as GameObject;
            rnd_velo = UnityEngine.Random.Range(1f, 2f);

            // GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

            // To make animation slightly diff for each otter
            // Animator anim = Clone.GetComponent<Animator>();
            // if (anim != null)
            // {
            //     float randomStart = UnityEngine.Random.Range(0f, 1f);
            //     anim.Play("Walk", 0, randomStart); 
            //     anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            // }
            Rigidbody cloneRb = Clone.GetComponentInChildren<Rigidbody>();
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = cloneRb != null ? cloneRb.gameObject : Clone.gameObject;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
        }

        while (counter < 2)
        {
            SpawnNPC_checkpoint3(-500, -166, -7, 7);
            SpawnNPC_checkpoint3(-500, -166, 13, 27);
            counter++;
        }
    }

    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

    IEnumerator spawnRandomNPCsRoutine_checkpoint3()
    {   
        int counter = 0;
        while (counter < 3)
        {
            // Spawn at random intervals between 1 and 3 seconds
            float randomTimer = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(randomTimer);
            SpawnNPC_checkpoint3(-500, -166, -7, 7);
            SpawnNPC_checkpoint3(-500, -166, 13, 27);
            counter++;
        }
    }

    void SpawnNPC_checkpoint3(float minX, float maxX, float minZ, float maxZ)
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
        Rigidbody cloneRb = Clone.GetComponentInChildren<Rigidbody>();
        Clone.AddComponent<NPCStraight_Ats>();
        Clone.GetComponent<NPCStraight_Ats>().gb = cloneRb != null ? cloneRb.gameObject : Clone.gameObject;
        Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
    }

}
