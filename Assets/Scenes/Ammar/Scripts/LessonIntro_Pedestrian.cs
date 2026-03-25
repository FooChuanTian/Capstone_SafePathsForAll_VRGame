using UnityEngine;

public class LessonIntro_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public string introMessage = "Stay on the pedestrian path. Do not enter the cycling lane.";

    // Optional — wire this up in Lesson 3 to trigger phone distraction after intro closes
    public PhoneDistraction_Pedestrian phoneDistraction;

    void Start()
    {
        if (tutorial != null)
        {
            tutorial.ShowTutorial(introMessage, 1);

            if (phoneDistraction != null)
                tutorial.onTutorialClosed = phoneDistraction.TriggerIntroDistraction;
        }
    }
}