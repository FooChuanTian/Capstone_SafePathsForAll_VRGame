using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraRig;         // OVRCameraRig root
    public Transform centerEyeAnchor;   // OVRCameraRig > TrackingSpace > CenterEyeAnchor

    private CharacterController _cc;
    private float _verticalVelocity;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- Left thumbstick: translate ---
        Vector2 leftStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Move relative to HMD facing direction (ignore vertical tilt)
        Vector3 forward = centerEyeAnchor.forward;
        Vector3 right   = centerEyeAnchor.right;
        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * leftStick.y + right * leftStick.x) * moveSpeed;

        // --- Gravity ---
        if (_cc.isGrounded)
            _verticalVelocity = -1f;          // small value keeps isGrounded stable
        else
            _verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = _verticalVelocity;

        _cc.Move(moveDir * Time.deltaTime);
    }
}