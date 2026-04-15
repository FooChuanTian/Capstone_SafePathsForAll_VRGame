using UnityEngine;
using System.Collections;

public class VREndPage : MonoBehaviour
{
    private bool acceptInput = false;
    void Start()
    {
        StartCoroutine(DelayedStart());
    }
    void Update()
    {
        StartPedestrianLesson startPedestrian = new StartPedestrianLesson();
        if (OVRInput.GetDown(OVRInput.Button.Three) && acceptInput)
        {
            startPedestrian.MoveOn();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four) && acceptInput)
        {
            startPedestrian.RepeatLesson();
        }
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        acceptInput = true;
    }

}
