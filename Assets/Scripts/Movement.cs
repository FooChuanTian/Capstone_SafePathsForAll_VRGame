using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Camera-Control/Movement")]

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 35.0f;

    private bool isGrounded = true;

    public float moveSpeed = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(jumpForce * 10 * Vector3.up, ForceMode.Acceleration);
        }

        if (Keyboard.current.aKey.isPressed) rb.AddForce(-moveSpeed * transform.right, ForceMode.Acceleration);
        if (Keyboard.current.dKey.isPressed) rb.AddForce(moveSpeed * transform.right, ForceMode.Acceleration);
        if (Keyboard.current.wKey.isPressed) rb.AddForce(moveSpeed * transform.forward, ForceMode.Acceleration);
        if (Keyboard.current.sKey.isPressed) rb.AddForce(-moveSpeed * transform.forward, ForceMode.Acceleration);

        // Press Up arrow key to zoom in the camera.
        if (Keyboard.current.upArrowKey.isPressed) Camera.main.fieldOfView -= 1;

        // Press Down arrow key to zoom out the camera.
        if (Keyboard.current.downArrowKey.isPressed) Camera.main.fieldOfView += 1;

        // Press Left arrow key to pan the camera to the left.
        if (Keyboard.current.leftArrowKey.isPressed) Camera.main.transform.Translate(-2, 0, 0);

        // Press Right arrow key to pan the camera to the right.
        if (Keyboard.current.rightArrowKey.isPressed) Camera.main.transform.Translate(2, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = false;
        }
    }
}
