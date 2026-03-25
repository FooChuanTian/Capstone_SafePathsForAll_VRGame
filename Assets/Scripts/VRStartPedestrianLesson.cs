using Unity.VisualScripting;
using UnityEngine;

public class VRStartPedestrianLesson : MonoBehaviour
{
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            StartPedestrianLesson startPedestrian = new StartPedestrianLesson();
            startPedestrian.StartLesson();
        }
    }
}
