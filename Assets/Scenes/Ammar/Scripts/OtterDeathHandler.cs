using System.Collections;
using UnityEngine;

public class OtterDeathHandler : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool collisionEnabled = false;

    public float collisionEnableDelay = 1f; // Wait before listening for collisions
    public float destroyDelay = 2f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(EnableCollisionAfterDelay());
    }

    IEnumerator EnableCollisionAfterDelay()
    {
        yield return new WaitForSeconds(collisionEnableDelay);
        collisionEnabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead || !collisionEnabled) return;

        // Only trigger death on collision with cyclist or obstacle tags
        if (collision.gameObject.CompareTag("cyclist") ||
            collision.gameObject.CompareTag("obstacle") ||
            collision.gameObject.CompareTag("Player"))
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}