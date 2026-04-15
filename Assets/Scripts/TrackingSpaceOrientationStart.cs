using UnityEngine;

public class TrackingSpaceOrientationStart : MonoBehaviour
{
    public Transform trackingSpaceObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trackingSpaceObj.localPosition = new Vector3(0, 10f, 0);
        trackingSpaceObj.localRotation = Quaternion.Euler(0, -90, 0);
    }
}
