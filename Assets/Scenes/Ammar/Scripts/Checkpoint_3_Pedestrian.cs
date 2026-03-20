using UnityEngine;

public class Checkpoint_3_Pedestrian : MonoBehaviour
{
    public TutorialPanel_Pedestrian tutorial;

    public GameObject npcPrefab;

    public Transform[] spawnPoints;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            Debug.Log("Checkpoint 3 triggered");

            // Show final lesson message
            tutorial.ShowTutorial("Be aware of others and return to the left after overtaking", 3);

            SpawnMultipleNPCs();
        }
    }

    void SpawnMultipleNPCs()
    {
        if (npcPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("NPC prefab or spawn points not assigned!");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawn = spawnPoints[i];

            GameObject npc = Instantiate(npcPrefab, spawn.position, spawn.rotation);

            // Randomize speed slightly
            NPCWalker walker = npc.GetComponent<NPCWalker>();
            if (walker != null)
            {
                walker.speed = Random.Range(1.5f, 3.5f);
            }
        }
    }
}