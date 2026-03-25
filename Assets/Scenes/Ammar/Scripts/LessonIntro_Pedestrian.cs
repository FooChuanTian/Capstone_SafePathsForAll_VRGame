using UnityEngine;

public class LessonIntro_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public string introMessage = "Stay on the pedestrian path. Do not enter the cycling lane.";

    void Start()
    {
        if (tutorial != null)
            tutorial.ShowTutorial(introMessage, 1);
    }
}