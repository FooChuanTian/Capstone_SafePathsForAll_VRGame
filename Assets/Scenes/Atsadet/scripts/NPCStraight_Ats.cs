using UnityEngine;

public class NPCStraight_Ats : MonoBehaviour
{
    public GameObject gb;
    private Rigidbody rb;
    public float velocity;

    void Start()
    {
        rb = gb.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            // This constantly ensures the pedestrian moves at your velocity
            // Moving along the X axis as per your original logic:
            rb.linearVelocity = new Vector3(velocity * 5, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
}
