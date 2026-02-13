using UnityEditor.Callbacks;
using UnityEngine;

public class SwerveBehaviour : MonoBehaviour
{
    public GameObject gb;
    private string curr_path;
    private Rigidbody rb;
    void Start()
    {
        rb = gb.GetComponent<Rigidbody>();
        rb.AddForce(transform.right*5, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
