using UnityEngine;

public class BikeCollisionImpact : MonoBehaviour
{
    public float impactForce = 500f;
    public float upwardForce = 200f;

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            rb.AddForce(direction.normalized * impactForce + Vector3.up * upwardForce, ForceMode.Impulse);
            UnityEngine.Debug.Log("Bike hit: " + collision.gameObject.name);
        }
    }
}