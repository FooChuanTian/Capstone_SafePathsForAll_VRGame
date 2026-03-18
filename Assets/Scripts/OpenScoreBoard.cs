using UnityEngine;

public class OpenScoreBoard : MonoBehaviour
{
    public Transform ScoreBoardObject;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            ScoreBoardObject.gameObject.SetActive(true);
        }
    }
}
