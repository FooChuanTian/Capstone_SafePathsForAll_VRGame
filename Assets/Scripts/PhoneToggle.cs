using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneToggle : MonoBehaviour
{
    public Transform PhoneImage;
    private bool phoneActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhoneImage.gameObject.SetActive(false);
        phoneActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            phoneActive = !phoneActive;
            PhoneImage.gameObject.SetActive(phoneActive);
        }
    }
}
