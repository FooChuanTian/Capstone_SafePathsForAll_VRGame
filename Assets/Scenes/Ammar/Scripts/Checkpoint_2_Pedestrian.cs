using UnityEngine;

public class Checkpoint_2_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;

    public GameObject npcPrefab;
    public Transform spawnPoint;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // Show tutorial
            tutorial.ShowTutorial("Keep left to allow others to pass", 2);

            // Spawn NPC
            if (npcPrefab != null && spawnPoint != null)
            {
                Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

}