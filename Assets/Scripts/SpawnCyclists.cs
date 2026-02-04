using UnityEngine;

public class SpawnCyclists : MonoBehaviour
{
    public GameObject CyclistObject;
    public Rigidbody rb;
    private GameObject Clone;
    public float timeToSpawn = 4f;
    public float FirstSpawn = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FirstSpawn -= Time.deltaTime;
        if (FirstSpawn <= 0f)
        {
            Clone = Instantiate(CyclistObject, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            cloneRb.AddForce(transform.right*10, ForceMode.VelocityChange);
            FirstSpawn = timeToSpawn;

        }
    }
}
