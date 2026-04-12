using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class VRStartCyclistLesson : MonoBehaviour
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
            StartCyclistLesson startPedestrian = new StartCyclistLesson();
            startPedestrian.LessonStartCyclist();
        }
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        acceptInput = true;
    }

}
