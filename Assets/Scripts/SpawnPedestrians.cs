using UnityEngine;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class SpawnPedestrians : MonoBehaviour
{
    public GameObject PedestrianObject;
    public Rigidbody rb;
    private GameObject Clone;
    public float FirstSpawn = 5f;
    public bool cont;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cont = true;
        //StopSpawn.AddListener(StopSpawning);
    }

    // Update is called once per frame
    void Update()
    {
        FirstSpawn -= Time.deltaTime;
        if (FirstSpawn <= 0f && cont)
        {
            Clone = Instantiate(PedestrianObject, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            //cloneRb.AddForce(transform.right*10, ForceMode.VelocityChange);
            int rnd_velo = UnityEngine.Random.Range(5, 9);
            int isSwerving = UnityEngine.Random.Range(0, 2);
            string thing = isSwerving.ToString();
            Debug.Log("isSwerving: "+ thing);
            cloneRb.linearVelocity = new Vector3(rnd_velo, 0, 0);
            float timeToSpawn = UnityEngine.Random.Range(10f, 15f);
            if (isSwerving == 1)
            {
                Debug.Log("Swerving Pedestrian spawned");
                //Vector3 acceleration = new Vector3(0, 0, 100);
                //cloneRb.AddForce(acceleration, ForceMode.Acceleration);
                Clone.AddComponent<SwerveBehaviour>();
                Clone.GetComponent<SwerveBehaviour>().gb = Clone.gameObject;
            }
            FirstSpawn = timeToSpawn;

        }
    }

    public void NormalPedestrian(GameObject Clone)
    {
        Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
        //cloneRb.AddForce(transform.right*10, ForceMode.VelocityChange);
        int rnd_velo = UnityEngine.Random.Range(3, 9);

        cloneRb.linearVelocity = new Vector3(rnd_velo, 0, 0);
    }

    public void SwervingPedestrian(GameObject Clone)
    {
        Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
        //cloneRb.AddForce(transform.right*10, ForceMode.VelocityChange);
        int rnd_velo = UnityEngine.Random.Range(10, 25);
        cloneRb.linearVelocity = new Vector3(rnd_velo, 0, 0);
        SwerveBehaviour swerveBehaviour = Clone.AddComponent<SwerveBehaviour>();
        swerveBehaviour.gb = Clone;
    }

    public void StopSpawning()
    {
        cont = false;
    }

    public void StartSpawning()
    {
        cont = true;
    }

}
