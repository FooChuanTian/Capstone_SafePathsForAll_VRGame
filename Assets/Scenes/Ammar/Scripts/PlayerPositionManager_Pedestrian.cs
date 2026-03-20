using UnityEngine;

public class PlayerPositionManager_Pedestrian : MonoBehaviour
{
    public Transform player;

    // Static = persists even after scene reload (same as cyclist system)
    public static Vector3 lastCheckpointPosition;
    public static bool hasCheckpoint = false;

    void Awake()
    {
        // When scene reloads, move player to last checkpoint
        if (hasCheckpoint && player != null)
        {
            player.position = lastCheckpointPosition;
            Debug.Log("Restored to checkpoint: " + lastCheckpointPosition);
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null || player == null) return;

        lastCheckpointPosition = checkpoint.position;
        hasCheckpoint = true;

        Debug.Log("Checkpoint saved at: " + lastCheckpointPosition);
    }

    // OPTIONAL: if you ever want to reset everything (e.g. restart lesson)
    public void ResetCheckpoint()
    {
        hasCheckpoint = false;
        lastCheckpointPosition = Vector3.zero;

        Debug.Log("Checkpoint reset");
    }
}