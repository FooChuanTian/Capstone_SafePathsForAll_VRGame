using UnityEngine;

public class Checkpoint_3_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

    public GameObject npcPrefab;      // Otter
    public Transform[] spawnPoints;   // NPC_Spawn_1, 2, 3

    public GameObject bikePrefab;     // Bike
    public Transform bikeSpawnPoint;  // NPC_Spawn_4

    public float npcSpeed = 0.8f;
    public float bikeSpeed = 5f;

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
            tutorial.ShowTutorial("Well done! Remember — red lane is for cyclists, pedestrian path is for you. Stay safe!", 3);
            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        // Spawn Otters on pedestrian path
        if (npcPrefab != null && spawnPoints != null)
        {
            foreach (Transform point in spawnPoints)
            {
                if (point == null) continue;

                GameObject npc = Instantiate(npcPrefab, point.position, point.rotation);
                NPCWalker walker = npc.GetComponent<NPCWalker>();
                if (walker != null)
                {
                    walker.direction = Vector3.left;
                    walker.speed = npcSpeed;
                }
            }
        }

        // Spawn Bike in cycling lane and add NPCWalker at runtime
        if (bikePrefab != null && bikeSpawnPoint != null)
        {
            GameObject bike = Instantiate(bikePrefab, bikeSpawnPoint.position, bikeSpawnPoint.rotation);

            // Add NPCWalker at runtime since bike prefab doesn't have it
            NPCWalker bikeWalker = bike.AddComponent<NPCWalker>();
            bikeWalker.direction = Vector3.left;
            bikeWalker.speed = bikeSpeed;

            Debug.Log("CP3 (L1): Bike spawned with NPCWalker added at runtime");
        }
    }
}