using UnityEngine;

public class Checkpoint_3_Pedestrian_L3 : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;
    public PhoneDistraction_Pedestrian phoneDistraction;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPositionManager_Pedestrian pm = other.GetComponent<PlayerPositionManager_Pedestrian>();
        if (pm != null && respawnPoint != null)
            pm.SetCheckpoint(respawnPoint);

        if (hasTriggered) return;
        hasTriggered = true;

        if (tutorial != null)
        {
            tutorial.ShowTutorial("Stay aware! Don't let your phone distract you from your surroundings.", 3);
            tutorial.onTutorialClosed = TriggerCP3Distraction;
        }
    }

    void TriggerCP3Distraction()
    {
        if (phoneDistraction != null)
            phoneDistraction.TriggerCP3Distraction();
    }
}