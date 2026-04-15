using System.Collections;
using UnityEngine;

public class VREndPageCyclist : MonoBehaviour
{
    private bool acceptInput = false;
    void Start()
    {
        StartCoroutine(DelayedStart());
    }
    void Update()
    {
        StartCyclistLesson startCyclist = new StartCyclistLesson();
        if (OVRInput.GetDown(OVRInput.Button.Three) && acceptInput)
        {
            startCyclist.LessonStartCyclist();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four) && acceptInput)
        {
            startCyclist.RepeatLesson();
        }
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        acceptInput = true;
    }

}
