using UnityEngine;
using UnityEngine.InputSystem;

public class BikeBellBehaviour : MonoBehaviour
{

    AudioSource BellSound;
    void Start()
    {
        BellSound = GetComponent<AudioSource>();
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Bike Bell Range");
        if (Keyboard.current.zKey.wasPressedThisFrame && (other.gameObject.CompareTag("pedestrian") || other.gameObject.CompareTag("cyclist")))
        {
            Rigidbody rb_other = other.gameObject.GetComponent<Rigidbody>();
            Vector3 pos_difference = other.gameObject.transform.position - transform.position;
            rb_other.AddForce(pos_difference*50, ForceMode.Acceleration);
        }
    }
    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            Debug.Log("Bike bell");
            BellSound.Play();
        }
    }
}
