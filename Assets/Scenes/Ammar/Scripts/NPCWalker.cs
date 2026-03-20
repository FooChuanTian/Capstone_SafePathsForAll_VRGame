using UnityEngine;

public class NPCWalker : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 direction = Vector3.forward;

    void Start()
    {
        Debug.Log("NPC spawned and moving");
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}