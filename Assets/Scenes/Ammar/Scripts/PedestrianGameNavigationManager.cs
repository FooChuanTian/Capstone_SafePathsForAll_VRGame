using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PedestrianGameNavigationManager : MonoBehaviour
{
    public static PedestrianGameNavigationManager Instance;

    public int currentLevelIndex = 0;

    [SerializeField]
    private string[] sceneSequence = {
        "Pedestrian_lesson1", "Pedestrian_lesson_endpage",
        "Pedestrian_lesson2", "Pedestrian_lesson_endpage",
        "Pedestrian_lesson3", "Pedestrian_lesson_endpage",
        "PedestrianSimulationScene"
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
            // End of all lessons � go back to player selection or main menu
            SceneManager.LoadScene("PedestrianSimulationScene");
        }
    }

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