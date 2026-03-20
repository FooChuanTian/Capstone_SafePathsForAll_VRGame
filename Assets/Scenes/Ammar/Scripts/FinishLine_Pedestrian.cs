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

        if (tutorial != null)
        {
            tutorial.ShowTutorial("Congrats on completing lesson 1!", 4);

        }
    }
}