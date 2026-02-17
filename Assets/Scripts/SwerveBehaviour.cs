using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Timers;

public class SwerveBehaviour : MonoBehaviour
{
    public GameObject gb;
    private string start_path;
    private Rigidbody rb;
    Vector3 pos_start;
    bool delayBool = false;
    float delay_remaining = 3f;
    string[] valid_paths = {"pedestrian_lane_left", "pedestrian_lane_right", "cycling_lane_right", "cycling_lane_left"};
    void Start()
    {
        rb = gb.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, 100), ForceMode.Acceleration);
    }

    void OnCollisionStay(Collision collision)
    {
        if (start_path == null && valid_paths.Contains(collision.gameObject.tag))
        {
            start_path = collision.gameObject.tag;
            pos_start = transform.position;
            Debug.Log("Clone initial lane: " + start_path);
        }
        if (valid_paths.Contains(collision.gameObject.tag) && start_path != collision.gameObject.tag)
        {
            delayBool = true;
            if (delay_remaining <= 0) {
                Debug.Log("Swerving...");
                float target_z = collision.gameObject.GetComponent<Transform>().position.z - pos_start.z;
                Vector3 pos_diff = new Vector3(0, 0, -target_z).normalized;
                rb.AddForce(pos_diff*30, ForceMode.Acceleration);
                delay_remaining = 5f;
                delayBool = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (delayBool) delay_remaining-=Time.deltaTime;
    }
}
