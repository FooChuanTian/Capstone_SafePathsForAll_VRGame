using UnityEngine;

public class CustomCameraRig : OVRCameraRig
{
    public void PublicUpdateAnchors(bool anchorPositionTracked, bool anchorRotationTracked)
    {
        UpdateAnchors(anchorPositionTracked, anchorRotationTracked); // calls the protected parent method
    }
}