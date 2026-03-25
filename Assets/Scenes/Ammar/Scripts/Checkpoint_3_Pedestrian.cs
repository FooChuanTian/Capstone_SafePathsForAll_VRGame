using UnityEngine;

public class Checkpoint_3_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

    public GameObject npcPrefab;
    public Transform spawnPointWith;      // NPC_Spawn_1 — walks same direction as player (Vector3.left)
    public Transform[] spawnPointsToward; // NPC_Spawn_2, 3 — walks towards player (Vector3.right)

    public GameObject bikePrefab;
    public Transform bikeSpawnPoint;

    public float npcSpeed = 10f;
    public float bikeSpeed = 20f;

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
            tutorial.ShowTutorial("Great job staying on the pedestrian path! But also watch out for other pedestrians and be aware of those walking towards you!", 3);
            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        // Spawn Otter walking with player (same direction)
        if (npcPrefab != null && spawnPointWith != null)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPointWith.position, spawnPointWith.rotation);
            NPCWalker walker = npc.AddComponent<NPCWalker>();
            walker.direction = Vector3.left;
            walker.speed = npcSpeed;
        }

        // Spawn Otters walking towards player
        if (npcPrefab != null && spawnPointsToward != null)
        {
            foreach (Transform point in spawnPointsToward)
            {
                if (point == null) continue;

                GameObject npc = Instantiate(npcPrefab, point.position, point.rotation);
                NPCWalker walker = npc.AddComponent<NPCWalker>();
                walker.direction = Vector3.right;
                walker.speed = npcSpeed;
            }
        }

        // Spawn Bike in cycling lane
        if (bikePrefab != null && bikeSpawnPoint != null)
        {
            GameObject bike = Instantiate(bikePrefab, bikeSpawnPoint.position, bikeSpawnPoint.rotation);
            NPCWalker bikeWalker = bike.AddComponent<NPCWalker>();
            bikeWalker.direction = Vector3.right;
            bikeWalker.speed = bikeSpeed;
            UnityEngine.Debug.Log("CP3 (L1): Bike spawned");
        }
    }
}