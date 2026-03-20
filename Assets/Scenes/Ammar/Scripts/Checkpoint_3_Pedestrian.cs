using UnityEngine;

public class Checkpoint_3_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

    public GameObject npcPrefab;
    public Transform[] spawnPoints;

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
            tutorial.ShowTutorial(
                "Be aware of others and return to the left after overtaking.",
                3
            );

            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        if (npcPrefab != null && spawnPoints != null)
        {
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Instantiate(npcPrefab, point.position, point.rotation);
                }
            }

            Debug.Log("CP3 NPCs spawned");
        }
    }
}