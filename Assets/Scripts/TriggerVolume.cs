using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    public UnityEvent TriggerEntered;
    public UnityEvent TriggerExited;

    public void OnTriggerEnter(Collider other)
    {
        TriggerEntered.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        TriggerExited.Invoke();
    }
}
