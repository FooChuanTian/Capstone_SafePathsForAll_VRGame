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
        new Vector3(-9, 1, 35),
        new Vector3(-11, 1, 43),
        new Vector3(-15, 1, 50),
        new Vector3(-7, 1, 54),
        new Vector3(-8, 1, 60),
        new Vector3(-9, 1, 64)
    };
    public GameObject pedestrianNPCObject;
    public TextMeshProUGUI GoalText;

    void Start()
    {
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        Debug.Log("TEST:Current scene: " + currentsceneName); // Debug log to check the current scene name
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && hasShownLaneTutorial == false)
        {   
            // cleanup();  //Disable for testing

            if (currentsceneName == "Cyclist_lesson1")  // Keep to cyclist lane
            {
                displayMessage = "Pedestrians ahead using the wrong lane!\nRing them and continue on the cyclist lane";
                GoalText.text = "Keep to the cyclist lane";
                runLesson1Stage2();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint2());
            }
            else if (currentsceneName == "Cyclist_lesson2") // Keep to left of cyclist lane
            {
                displayMessage = "Look out for obstacles ahead!\nAvoid them and continue on the left side of the cyclist lane";
                GoalText.text = "Keep to the left side of the cyclist lane";
                runLesson2Stage2();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint2());
            }
            else if (currentsceneName == "Cyclist_lesson3") // Follow the tactile strips
            {
                displayMessage = "Obstacles may be hard to tell from afar\nTactile strips help to alert you earlier so you have ample time to react";
                GoalText.text = "Follow the tactile strips to navigate crowded areas safely";
                runLesson3Stage2();
                StartCoroutine(spawnRandomNPCsRoutine_checkpoint2());
            }
            tutorial.ShowTutorial(displayMessage, 2);
            hasShownLaneTutorial = true; // Ensures it only freezes the game once

            // Update the manager on the player
            PlayerPositionManager_Ats positionManager = collision.GetComponent<PlayerPositionManager_Ats>();
            if (positionManager != null)
            {
                positionManager.UpdatePreviousPosition(checkPointlocation_2);
            }
        }
    }

    void runLesson1Stage2()
    {   
        int counter = 0;
        for (int i = spawnPoints.Count - 1; i >= 0; i--)
        {
            Vector3 spawnPoint = spawnPoints[i];
            Clone = Instantiate(PedestrianObject, spawnPoint, new Quaternion(0, -0.90f, 0, 1)) as GameObject;

            // GameNavigationManager.Instance.activeClones.Add(Clone); //Disable for testing

            // To make animation slightly diff for each otter
            Animator anim = Clone.GetComponent<Animator>();
            if (anim != null)
            {
                float randomStart = UnityEngine.Random.Range(0f, 1f);
                anim.Play("Walk", 0, randomStart); 
                anim.speed = UnityEngine.Random.Range(0.8f, 1.2f);
            }
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            // float rnd_velo = UnityEngine.Random.Range(-0.5f, -1.0f);
            float rnd_velo = -0.5f;
            Clone.AddComponent<NPCStraight_Ats>();
            Clone.GetComponent<NPCStraight_Ats>().gb = Clone.gameObject;
            Clone.GetComponent<NPCStraight_Ats>().velocity = rnd_velo;
        }

        while (counter < 3)
        {
            SpawnNPC_checkpoint2(-166, 160, -7, 7);
            SpawnNPC_checkpoint2(-166, 160, 13, 27);
            counter++;
        }
    }

    void runLesson2Stage2()
    {   
        int counter = 0;
        while (counter < 3)
        {
            SpawnNPC_checkpoint2(-166, 160, -7, 7);
            SpawnNPC_checkpoint2(-166, 160, 13, 27);
            counter++;
        }
    }

    void runLesson3Stage2()
    {   
        int counter = 0;
        while (counter < 3)
        {
            SpawnNPC_checkpoint2(-166, 160, -7, 7);
            SpawnNPC_checkpoint2(-166, 160, 13, 27);
            counter++;
        }
    }
    public void cleanup()
    {
        GameNavigationManager.Instance.Cleanup();
    }

    IEnumerator spawnRandomNPCsRoutine_checkpoint2()
    {   
        int counter = 0;
        while (counter < 3)
        {
            // Spawn at random intervals between 1 and 3 seconds
            float randomTimer = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(randomTimer);
            SpawnNPC_checkpoint2(-166, 160, -7, 7);
            SpawnNPC_checkpoint2(-166, 160, 13, 27);
            counter++;
        }
    }

    void SpawnNPC_checkpoint2(float minX, float maxX, float minZ, float maxZ)
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
// coordinates: 
// 0,0,35
// -11, 0, 43
// -15,0,50
// -7,0,54
// -9,0,64

// cehckpoint wall at x = 167 to -500
// left pedestrian lane (z = -7 to =7)
// right pedestrian lane (z = 13 t0 27)
// left cyclist lane (z =33 to 47)
// right cyclist lane (z = 53 to 67)