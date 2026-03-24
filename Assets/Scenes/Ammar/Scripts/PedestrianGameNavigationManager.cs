using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PedestrianGameNavigationManager : MonoBehaviour
{
    public static PedestrianGameNavigationManager Instance;

    public int currentLevelIndex = 0;

    [SerializeField]
    private string[] sceneSequence = {
        "Pedestrian_lesson1", "Pedestrian_lesson_endpage",
        "Pedestrian_lesson2", "Pedestrian_lesson_endpage",
        "Pedestrian_lesson3", "Pedestrian_lesson_endpage"
    };

    public List<GameObject> activeClones = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called by finish line tutorial to move to end page
    public void LoadNextScene()
    {
        if (currentLevelIndex < sceneSequence.Length)
        {
            string sceneToLoad = sceneSequence[currentLevelIndex];
            currentLevelIndex++;
            Debug.Log("[Pedestrian] Loading: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Called by "Move On" button on end page — already advanced by LoadNextScene so just load next
    public void LoadNextLesson()
    {
        LoadNextScene();
    }

    // Called by "Replay" button on end page — go back 2 steps to reload the lesson (not the end page)
    public void ReloadCurrentScene()
    {
        if (currentLevelIndex >= 2)
        {
            string sceneToLoad = sceneSequence[currentLevelIndex - 2];
            Debug.Log("[Pedestrian] Reloading lesson: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // Fallback
            SceneManager.LoadScene("Pedestrian_lesson1");
        }
    }

    public void Cleanup()
    {
        foreach (GameObject clone in activeClones)
        {
            if (clone != null) Destroy(clone);
        }
        activeClones.Clear();
    }
}