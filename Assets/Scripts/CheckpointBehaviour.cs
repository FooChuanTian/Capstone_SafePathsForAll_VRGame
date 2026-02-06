using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    public Transform CurrentCheckpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject SpawnPrevious;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            PlayerPositionManager playerPositionManager = player.GetComponent<PlayerPositionManager>();
            playerPositionManager.PreviousCheckpoint = CurrentCheckpoint;
            SpawnCyclists spawnCyclists = SpawnPrevious.GetComponent<SpawnCyclists>();
            spawnCyclists.StopSpawning();
        }
    }
}
