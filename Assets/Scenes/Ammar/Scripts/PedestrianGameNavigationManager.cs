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

    public void ReloadCurrentScene()
    {
        if (currentLevelIndex > 0)
        {
            string sceneToLoad = sceneSequence[currentLevelIndex - 1];
            SceneManager.LoadScene(sceneToLoad);
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