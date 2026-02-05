using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class PlayerCollisionHandler : MonoBehaviour
{
    private bool isCyclingPath = false;
    public TextMeshProUGUI WhichLaneText;
    public Transform Checkpoint0;
    private List<Transform> CheckpointList = new List<Transform>();
    public Transform Player;

    void Start()
    {

        Player.transform.position = Checkpoint0.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cyclist"))
        {
            Debug.Log("Collided with cyclist");
            PlayerPositionManager positionManager = Player.gameObject.GetComponent<PlayerPositionManager>();
            positionManager.Teleport();
        }
        else if (collision.gameObject.CompareTag("checkpoint"))
        {
            Debug.Log("Checkpoint reached!");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("cycling_lane_left"))
        {
            isCyclingPath = true;
            WhichLaneText.text = "Cycling Lane Left";
            Debug.Log("On cycling path!");
        }
        else if (collision.gameObject.CompareTag("cycling_lane_right"))
        {
            isCyclingPath = true;
            WhichLaneText.text = "Cycling Lane Right";
            Debug.Log("On right cycling path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            isCyclingPath = false;
            WhichLaneText.text = "Pedestrian Lane Left";
            Debug.Log("On left pedestrian path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isCyclingPath = false;
            WhichLaneText.text = "Pedestrian Lane Right";
            Debug.Log("On right pedestrian path!");
        }
    }

    public void GameOver()
    {
        
    }
}
