using UnityEngine;

public class StartPedestrianLesson : MonoBehaviour
{
    public void StartLesson()
    {
        PedestrianGameNavigationManager.Instance.currentLevelIndex = 0;
        PedestrianGameNavigationManager.Instance.LoadNextScene();
    }

    public void RepeatLesson()
    {
        PedestrianGameNavigationManager.Instance.ReloadCurrentScene();
    }
}