using UnityEngine;

public class Checkpoint_3_Pedestrian_L3 : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public TutorialPanel_Pedestrian tutorial_vr;
    public Transform respawnPoint;
    public PhoneDistraction_Pedestrian phoneDistraction;
    public Transform trackingSpaceObj;
    private string tutorial_message = "Stay aware! Don't let your phone distract you from your surroundings.";

    private bool hasTriggered = false;
    void Start()
    {
        trackingSpaceObj.localPosition = new Vector3(0, 10f, 0);
        trackingSpaceObj.localRotation = Quaternion.Euler(0, -90, 0);
    }

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
            tutorial.ShowTutorial(tutorial_message, 3);
            tutorial.onTutorialClosed = TriggerCP3Distraction;
        }
        if (tutorial_vr != null)
        {
            tutorial_vr.ShowTutorial(tutorial_message, 3);
            tutorial_vr.onTutorialClosed = TriggerCP3Distraction;
        }
    }

    void TriggerCP3Distraction()
    {
        if (phoneDistraction != null)
            phoneDistraction.TriggerCP3Distraction();
    }
}