using UnityEngine;

public class FinishLine_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;

    private bool hasFinished = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasFinished) return;

        hasFinished = true;

        Debug.Log("Player reached finish line");

        // Reset checkpoint so "practice again" starts from the beginning
        PlayerPositionManager_Pedestrian.hasCheckpoint = false;
        PlayerPositionManager_Pedestrian.lastCheckpointPosition = Vector3.zero;

        if (tutorial != null)
        {
            tutorial.ShowTutorial("Congrats on completing lesson 1!", 4);
        }
    }
}