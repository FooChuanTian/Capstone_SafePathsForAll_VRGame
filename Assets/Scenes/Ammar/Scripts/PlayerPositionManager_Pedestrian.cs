using UnityEngine;

public class PlayerPositionManager_Pedestrian : MonoBehaviour
{
    public Transform Player;

    // Store last safe position (checkpoint)
    private Vector3 lastSafePosition;
    private bool hasCheckpoint = false;

    void Start()
    {
        // Set initial spawn as safe position
        if (Player != null)
        {
            lastSafePosition = Player.position;
            hasCheckpoint = true;
        }
    }

    // Call this when reaching checkpoint
    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint != null)
        {
            lastSafePosition = checkpoint.position;
            hasCheckpoint = true;
            Debug.Log("[Pedestrian] Checkpoint saved at: " + lastSafePosition);
        }
    }

    // Teleport player back to last checkpoint
    public void Respawn()
    {
        if (hasCheckpoint && Player != null)
        {
            Player.position = lastSafePosition;
            Debug.Log("[Pedestrian] Respawned at checkpoint");
        }
    }

    // Optional: reset checkpoint (e.g. at end of lesson)
    public void ResetCheckpoint(Transform startPoint)
    {
        if (startPoint != null)
        {
            lastSafePosition = startPoint.position;
            hasCheckpoint = true;
        }
    }
}