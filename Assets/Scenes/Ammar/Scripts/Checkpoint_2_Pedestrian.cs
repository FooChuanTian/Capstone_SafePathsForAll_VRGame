using UnityEngine;
using UnityEngine.XR;

public class Checkpoint_2_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public TutorialPanel_Pedestrian tutorial_vr;
    public Transform respawnPoint;

    public GameObject npcPrefab;        // Otter
    public Transform otterSpawnPoint;   // NPC_Spawn_CP2_Otter (on cycling lane, stationary)

    public GameObject bikePrefab;       // Bike
    public Transform bikeSpawnPoint;    // NPC_Spawn_CP2_Bike (further up cycling lane)
    public float bikeSpeed = 100f;

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
            tutorial.ShowTutorial("The cycling lane may look empty, but bikes can zoom in at any time. Never walk on the red lane!", 2);
            tutorial.onTutorialClosed = RunCheckpointLogic;
        }
        if (tutorial_vr != null && XRSettings.enabled)
        {
            tutorial_vr.ShowTutorial("The cycling lane may look empty, but bikes can zoom in at any time. Never walk on the red lane!", 2);
            tutorial_vr.onTutorialClosed = RunCheckpointLogic;
        }
    }

    void RunCheckpointLogic()
    {
        // Spawn stationary Otter on cycling lane
        if (npcPrefab != null && otterSpawnPoint != null)
        {
            Instantiate(npcPrefab, otterSpawnPoint.position, otterSpawnPoint.rotation);
            UnityEngine.Debug.Log("CP2 (L1): Stationary Otter spawned on cycling lane");
        }

        // Spawn bike zooming in to hit the Otter
        if (bikePrefab != null && bikeSpawnPoint != null)
        {
            GameObject bike = Instantiate(bikePrefab, bikeSpawnPoint.position, bikeSpawnPoint.rotation);
            NPCWalker bikeWalker = bike.AddComponent<NPCWalker>();
            bikeWalker.direction = Vector3.right;
            bikeWalker.speed = bikeSpeed;
            UnityEngine.Debug.Log("CP2 (L1): Bike spawned zooming towards Otter");
        }
    }
}