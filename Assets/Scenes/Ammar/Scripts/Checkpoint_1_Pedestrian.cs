using UnityEngine;

public class Checkpoint_1_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered: " + other.name);

        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER HIT CHECKPOINT 1");

            tutorial.gameObject.SetActive(true);

            tutorial.ShowTutorial(
                "Stay on the pedestrian path. Avoid the cycling lane.",
                1
            );

            hasTriggered = true;
        }
    }
}