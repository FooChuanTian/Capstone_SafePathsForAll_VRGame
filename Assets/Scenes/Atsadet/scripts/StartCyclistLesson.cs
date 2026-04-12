using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCyclistLesson : MonoBehaviour
{
public void LessonStartCyclist()
    {
        // SceneManager.LoadScene("Cyclist_lesson1");
        GameNavigationManager.Instance.LoadNextDynamicScene();
    }

public void RepeatLesson()
    {
        GameNavigationManager.Instance.LoadCurrentLessonScene();
    }

}