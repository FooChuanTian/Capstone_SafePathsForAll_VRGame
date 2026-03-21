using UnityEngine;

public class Checkpoint_2_Pedestrian_L3 : MonoBehaviour
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
            tutorial.ShowTutorial("Watch out! Distracted walking can lead to collisions. Put your phone away!", 2);
            tutorial.onTutorialClosed = TriggerDangerDistraction;
        }
    }

    void TriggerDangerDistraction()
    {
        if (phoneDistraction != null)
            phoneDistraction.TriggerDangerDistraction(phoneDistraction.npcSpawnPoint_CP2);
    }
}