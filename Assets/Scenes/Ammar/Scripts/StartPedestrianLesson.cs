using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPedestrianLesson : MonoBehaviour
{
    // Called ONLY from the instruction screen — resets and starts from lesson 1
    public void StartLesson()
    {
        if (PedestrianGameNavigationManager.Instance != null)
        {
            PedestrianGameNavigationManager.Instance.currentLevelIndex = 0;
            PedestrianGameNavigationManager.Instance.LoadNextScene();
        }
        else
        {
            Debug.LogWarning("[Pedestrian] NavigationManager Instance is null! Loading lesson directly.");
            SceneManager.LoadScene("Pedestrian_lesson1");
        }
    }

    // Called by "Move On" button on end page — progresses to next lesson
    public void MoveOn()
    {
        if (PedestrianGameNavigationManager.Instance != null)
        {
            PedestrianGameNavigationManager.Instance.LoadNextScene();
        }
        else
        {
            Debug.LogWarning("[Pedestrian] NavigationManager Instance is null! Loading lesson directly.");
            SceneManager.LoadScene("Pedestrian_lesson1");
        }
    }

    // Called by "Replay" button on end page — reloads the current lesson
    public void RepeatLesson()
    {
        if (PedestrianGameNavigationManager.Instance != null)
        {
            PedestrianGameNavigationManager.Instance.ReloadCurrentScene();
        }
        else
        {
            Debug.LogWarning("[Pedestrian] NavigationManager Instance is null! Reloading lesson directly.");
            SceneManager.LoadScene("Pedestrian_lesson1");
        }
    }
}