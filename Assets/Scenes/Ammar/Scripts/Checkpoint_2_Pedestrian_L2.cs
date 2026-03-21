using UnityEngine;

public class Checkpoint_2_Pedestrian_L2 : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public Transform respawnPoint;

    public GameObject npcPrefab;
    public Transform[] spawnPoints;

    public float npcSpeed = 1.5f;

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
            tutorial.ShowTutorial("Pedestrians coming the other way! Keep left so they can pass safely.", 2);
            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        if (npcPrefab == null || spawnPoints == null) return;

        foreach (Transform point in spawnPoints)
        {
            if (point == null) continue;

            GameObject npc = Instantiate(npcPrefab, point.position, point.rotation);

            // Add NPCWalker at runtime since Otter prefab doesn't have it
            NPCWalker walker = npc.AddComponent<NPCWalker>();
            walker.direction = Vector3.right;
            walker.speed = npcSpeed;
        }
    }
}