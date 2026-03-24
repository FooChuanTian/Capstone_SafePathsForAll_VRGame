using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string NextLevelString;

    public void StartNextLevel()
    {
        Time.timeScale = 1;
        if (GameNavigationManager.Instance || PedestrianGameNavigationManager.Instance)
        {
            SceneManager.LoadScene("FinalScene");
        }
        else 
        {
            SceneManager.LoadScene(NextLevelString);
        }
    }
}
