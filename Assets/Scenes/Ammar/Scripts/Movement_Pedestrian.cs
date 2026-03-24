using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Camera-Control/Movement_Pedestrian")]

public class Movement_Pedestrian : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 35.0f;
    public float moveSpeed = 10.0f;

    // Max speed cap for pedestrian — prevents cheesing through the level
    public float maxSpeed = 10.0f;

    private bool isGrounded = true;

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            rb.AddForce(jumpForce * 10 * Vector3.up, ForceMode.Acceleration);

        if (Keyboard.current.aKey.isPressed) rb.AddForce(-moveSpeed * transform.right, ForceMode.Acceleration);
        if (Keyboard.current.dKey.isPressed) rb.AddForce(moveSpeed * transform.right, ForceMode.Acceleration);
        if (Keyboard.current.wKey.isPressed) rb.AddForce(moveSpeed * transform.forward, ForceMode.Acceleration);
        if (Keyboard.current.sKey.isPressed) rb.AddForce(-moveSpeed * transform.forward, ForceMode.Acceleration);

        // Clamp horizontal speed — ignore Y so jumping still works normally
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 capped = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(capped.x, rb.linearVelocity.y, capped.z);
        }

        if (Keyboard.current.upArrowKey.isPressed) Camera.main.fieldOfView -= 1;
        if (Keyboard.current.downArrowKey.isPressed) Camera.main.fieldOfView += 1;
        if (Keyboard.current.leftArrowKey.isPressed) Camera.main.transform.Translate(-2, 0, 0);
        if (Keyboard.current.rightArrowKey.isPressed) Camera.main.transform.Translate(2, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}