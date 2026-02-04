using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerGroundCollision : MonoBehaviour
{
    private bool isCyclingPath = false;
    public TextMeshProUGUI WhichLaneText;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cyclist"))
        {
            Debug.Log("Collided with cyclist");
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("cycling_lane"))
        {
            isCyclingPath = true;
            WhichLaneText.text = "Cycling Lane";
            Debug.Log("On cycling path!");
        }
        else if (collision.gameObject.CompareTag("pedestrian_lane"))
        {
            isCyclingPath = false;
            WhichLaneText.text = "Pedestrian Lane";
            Debug.Log("On pedestrian path!");
        }
    }

    public void GameOver()
    {
        
    }
}
