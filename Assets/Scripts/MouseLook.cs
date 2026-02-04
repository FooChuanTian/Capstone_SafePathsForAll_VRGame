using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Camera-Control/Mouse Look")]

public class MouseLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float sensitivityX = 2F;
    public float sensitivityY = 2F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    private bool isMouseActive = true;
    private float rotationY = 0F;



    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 delta = Mouse.current.delta.ReadValue();
        float mouseX = delta.x;
        float mouseY = delta.y;

        if (isMouseActive)
        {
            float rotationX = transform.localEulerAngles.y + mouseX * sensitivityX;

            rotationY += mouseY * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isMouseActive = !isMouseActive;

            if (isMouseActive == true) Debug.Log("Mouse Active!");
            else Debug.Log("Mouse Inactive!");
        }
    }
}