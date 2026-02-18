using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPedestrianGame : MonoBehaviour
{
    public void GameStartPedestrian()
    {
        SceneManager.LoadScene("PedestrianSimulationScene");
    }
}
