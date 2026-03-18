using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string NextLevelString;

    public void StartNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(NextLevelString);
    }
}
