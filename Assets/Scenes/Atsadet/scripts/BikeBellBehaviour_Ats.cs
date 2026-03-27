using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BikeBellBehaviour_Ats : MonoBehaviour
{

    AudioSource BellSound;
    private HashSet<GameObject> reactedObjects = new HashSet<GameObject>();
    private string currentsceneName;
    private bool isZPressed = false;
    void Start()
    {
        BellSound = GetComponent<AudioSource>();
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Bike Bell Range");
        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
        // if (Keyboard.current.zKey.wasPressedThisFrame && (other.gameObject.CompareTag("pedestrian") || other.gameObject.CompareTag("cyclist")))
        if (isZPressed && (other.gameObject.CompareTag("pedestrian") || other.gameObject.CompareTag("cyclist")))
        {   
            if (currentsceneName == "Cyclist_lesson1" && other.gameObject.CompareTag("cyclist"))
            {
                Debug.Log("Bike_lesson 1 dont react to bell");
            } 
            else{
                other.gameObject.GetComponent<SetTextAcive>().SetTextActive();
                Rigidbody rb_other = other.gameObject.GetComponent<Rigidbody>();
                //Vector3 pos_difference = other.gameObject.transform.position - transform.position;
                Vector3 pos_difference = new Vector3(0, 0, other.gameObject.transform.position.z - transform.position.z);
                Debug.Log("Pos difference before adjustment: " + pos_difference);
                // if (pos_difference.z == 0)
                if (Mathf.Abs(pos_difference.z) < 10f)
                {
                    pos_difference.z = 10f * Mathf.Sign(pos_difference.z);
                }
                //rb_other.AddForce(pos_difference*30, ForceMode.Acceleration);
                StartCoroutine(MoveNPC(rb_other, pos_difference));
                Debug.Log("Bike current scene");
            }
        }
    }
    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            Debug.Log("Bike bell");
            BellSound.Play();
            StartCoroutine(TriggerBellSignal());
        }
    }
    private IEnumerator MoveNPC(Rigidbody rb, Vector3 pos_difference)
    {
        yield return new WaitForSeconds(1f);
        rb.AddForce(pos_difference*30, ForceMode.Acceleration);
    }
    IEnumerator TriggerBellSignal()
    {
        isZPressed = true;
        yield return new WaitForFixedUpdate();
        isZPressed = false;
    }
}
