using UnityEngine;

public class PedestrianStraight : MonoBehaviour
{
    public GameObject gb;
    private Rigidbody rb;
    public int velocity;

    void Start()
    {
        rb = gb.GetComponent<Rigidbody>();
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
