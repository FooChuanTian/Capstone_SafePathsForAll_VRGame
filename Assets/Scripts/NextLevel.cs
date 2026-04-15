using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

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
    public void Update()
    {
        if (XRSettings.enabled)
        {
            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                StartNextLevel();
            }
        }
    }
}
