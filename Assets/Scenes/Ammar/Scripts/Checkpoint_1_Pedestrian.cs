using UnityEngine;

public class Checkpoint_1_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;
    public TutorialPanel_Pedestrian tutorial_vr;
    public Transform respawnPoint;
    public Transform trackingSpaceObj;

    private bool hasTriggered = false;

    void Start()
    {
        trackingSpaceObj.localPosition = new Vector3(0, 10f, 0);
        trackingSpaceObj.localRotation = Quaternion.Euler(0, -90, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPositionManager_Pedestrian pm = other.GetComponent<PlayerPositionManager_Pedestrian>();
        if (pm != null && respawnPoint != null)
            pm.SetCheckpoint(respawnPoint);

        if (hasTriggered) return;
        hasTriggered = true;

    }
}