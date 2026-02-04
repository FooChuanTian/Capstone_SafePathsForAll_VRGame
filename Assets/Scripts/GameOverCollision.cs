using UnityEngine;

public class GameOverCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cyclist"))
        {
            Debug.Log("Collided with player!");
        }
    }
}
