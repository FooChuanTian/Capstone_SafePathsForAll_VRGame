using UnityEngine;
using UnityEngine.XR;

public class Checkpoint_0 : MonoBehaviour
{   
    public Rigidbody rb;
    public Rigidbody rb_vr;
    private Rigidbody rb_curr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (XRSettings.enabled)
        {
            rb_curr = rb_vr;
        }
        else
        {
            rb_curr = rb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The wall only moves if the player is further ahead than the wall
        if (rb_curr.position.x < transform.position.x - 10f)
        {
            // transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y, rb.position.z);
            transform.position = transform.position - new Vector3(1f, 0, 0);
        }
    }
}
