using UnityEngine;

public class Checkpoint_0 : MonoBehaviour
{   
    public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // The wall only moves if the player is further ahead than the wall
        if (rb.position.x < transform.position.x - 10f)
        {
            // transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y, rb.position.z);
            transform.position = transform.position - new Vector3(1f, 0, 0);
        }
    }
}
