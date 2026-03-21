using UnityEngine;

public class Checkpoint_1_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

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
            tutorial.ShowTutorial("Welcome! The red lane is for cyclists only. Always stay on the pedestrian path.", 1);
    }
}