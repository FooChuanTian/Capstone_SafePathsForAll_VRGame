using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCyclistGame : MonoBehaviour
{
public void GameStartCyclist()
    {
        SceneManager.LoadScene("CyclistSimulationScene");
    }
}
