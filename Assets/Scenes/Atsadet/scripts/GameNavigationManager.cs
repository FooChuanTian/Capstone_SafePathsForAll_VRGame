using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameNavigationManager : MonoBehaviour
{
    // Static instance allows other scripts to access this easily
    public static GameNavigationManager Instance;

    [Header("Game State")]
    public int currentLevelIndex = 0;
    
    // A list of scene names in the order you want them to load
    [SerializeField] private string[] sceneSequence = { 
        "Cyclist_lesson1", "Cyclist_lesson_endpage", 
        "Cyclist_lesson2", "Cyclist_lesson_endpage",
        "Cyclist_lesson3", "Cyclist_lesson_endpage", "CyclistSimulationScene"};

    public List<GameObject> activeClones = new List<GameObject>();

    private void Awake()
    {
        // Singleton Pattern: Ensures only one Manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This keeps the object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This is the function your Button will call
    public void LoadNextDynamicScene()
    {
        if (currentLevelIndex < sceneSequence.Length)
        {
            string sceneToLoad = sceneSequence[currentLevelIndex];
            
            // Increment the index so the NEXT time we come back, it's different
            currentLevelIndex++;
            Debug.Log("Loading scene: " + sceneToLoad);
            Debug.Log("Next level index will be: " + currentLevelIndex);


            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("No more scenes in the sequence! Returning to Main Menu.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoadCurrentLessonScene()
    {
        if (currentLevelIndex > 0 && currentLevelIndex <= sceneSequence.Length)
        {   
            currentLevelIndex = currentLevelIndex - 2; // Decrement to get the current scene index
            string sceneToLoad = sceneSequence[currentLevelIndex]; // Load the current scene again

            currentLevelIndex++; // Increment back to the next scene for future calls
            Debug.Log("Reloading current lesson scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("Invalid level index!");
        }
    }

    public string getCurrentSceneName() {
        if (currentLevelIndex > 0 && currentLevelIndex <= sceneSequence.Length)
        {   
            return sceneSequence[currentLevelIndex - 1]; // Return the current scene name
        }
        else
        {
            Debug.Log("Invalid level index!");
            return null;
        }
    }

    public void Cleanup() {
        foreach (GameObject clone in activeClones) {
            if (clone != null) Destroy(clone);
        }
        activeClones.Clear();
    }
}