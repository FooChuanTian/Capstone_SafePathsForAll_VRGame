using UnityEngine;
using UnityEngine.XR;

public class Checkpoint_1_Pedestrian_L3 : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public TutorialPanel_Pedestrian tutorial_vr;
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

        if (tutorial != null && !XRSettings.enabled)
        {
            tutorial.onTutorialClosed = TriggerIntroDistraction;
        }

        if (tutorial_vr != null && XRSettings.enabled)
        {
            tutorial_vr.onTutorialClosed = TriggerIntroDistraction;
        }
    }

    void TriggerIntroDistraction()
    {
        if (phoneDistraction != null)
            phoneDistraction.TriggerIntroDistraction();
    }
}