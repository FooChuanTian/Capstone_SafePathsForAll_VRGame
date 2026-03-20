using UnityEngine;

public class Checkpoint_2_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

    public GameObject npcPrefab;
    public Transform spawnPoint;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPositionManager_Pedestrian pm = other.GetComponent<PlayerPositionManager_Pedestrian>();
        if (pm != null && respawnPoint != null)
        {
            pm.SetCheckpoint(respawnPoint);
        }

        if (hasTriggered) return;
        hasTriggered = true;

        if (tutorial != null)
        {
            tutorial.ShowTutorial("Keep left to allow others to pass.", 2);
            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        if (npcPrefab != null && spawnPoint != null)
        {
            Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("CP2 NPC spawned");
        }
    }
}