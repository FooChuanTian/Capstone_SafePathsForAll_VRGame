using UnityEngine;
using UnityEngine.SceneManagement;

public class VRPlayerSelect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            SceneManager.LoadScene("CyclistSimulationScene");
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            SceneManager.LoadScene("PedestrianSimulationScene");
        }
    }
}
