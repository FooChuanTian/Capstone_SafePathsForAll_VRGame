using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class VRStartPedestrianLesson : MonoBehaviour
{
    private bool acceptInput = false;
    void Start()
    {
        StartCoroutine(DelayedStart());
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) && acceptInput)
        {
            StartPedestrianLesson startPedestrian = new StartPedestrianLesson();
            startPedestrian.StartLesson();
        }
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        acceptInput = true;
    }

}
