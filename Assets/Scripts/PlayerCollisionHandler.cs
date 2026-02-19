using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class PlayerCollisionHandler : MonoBehaviour
{
    private bool isCyclingPath = false;
    public TextMeshProUGUI InstructionText;
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
            InstructionText.text = "Cycling Lane Left";
            Debug.Log("On cycling path!");
        }
        else if (collision.gameObject.CompareTag("cycling_lane_right"))
        {
            isCyclingPath = true;
            InstructionText.text = "Cycling Lane Right";
            Debug.Log("On right cycling path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_left"))
        {
            isCyclingPath = false;
            InstructionText.text = "Good! Stay left.";
            InstructionText.color = Color.green;
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane_right"))
        {
            isCyclingPath = false;
            InstructionText.text = "Keep left on shared paths.";
            InstructionText.color = Color.red;
        }
    }

    public void GameOver()
    {
        
    }
}
