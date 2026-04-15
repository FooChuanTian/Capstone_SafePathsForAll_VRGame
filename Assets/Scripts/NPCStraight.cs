using UnityEngine;

public class NPCStraight : MonoBehaviour
{
    public GameObject gb;
    private Rigidbody rb;
    public int velocity;

    void Start()
    {
        rb = gb.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(velocity, 0, 0), ForceMode.VelocityChange);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < velocity)
        {
            rb.linearVelocity = velocity * rb.linearVelocity.normalized;
        }
    }
}
