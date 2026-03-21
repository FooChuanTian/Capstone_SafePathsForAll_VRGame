using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;

    private bool hasFinished = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasFinished) return;

        hasFinished = true;

        // Reset checkpoint so "practice again" starts from the beginning
        PlayerPositionManager_Pedestrian.hasCheckpoint = false;
        PlayerPositionManager_Pedestrian.lastCheckpointPosition = Vector3.zero;

        string sceneName = SceneManager.GetActiveScene().name;
        string message = "Congrats on completing the lesson!";

        if (sceneName == "Pedestrian_lesson1")
            message = "Congrats on completing Lesson 1!";
        else if (sceneName == "Pedestrian_lesson2")
            message = "Congrats on completing Lesson 2!";
        else if (sceneName == "Pedestrian_lesson3")
            message = "Congrats on completing Lesson 3!";

        Debug.Log("Player reached finish line in " + sceneName);

        if (tutorial != null)
            tutorial.ShowTutorial(message, 4);
    }
}