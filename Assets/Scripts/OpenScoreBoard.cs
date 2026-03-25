using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

public class OpenScoreBoard : MonoBehaviour
{
    public Transform ScoreBoardObject;
    public Transform ScoreBoardVR;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            if (XRSettings.enabled)
            {
                ScoreBoardVR.gameObject.SetActive(true);
            }
            else 
            {
                ScoreBoardObject.gameObject.SetActive(true);
            }
        }
    }
}
