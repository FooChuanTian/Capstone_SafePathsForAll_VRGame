using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPedestrianLesson : MonoBehaviour
{
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