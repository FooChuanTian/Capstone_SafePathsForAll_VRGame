using UnityEngine;
using UnityEngine.Events;
using System;

public class SpawnCyclists : MonoBehaviour
{
    public GameObject CyclistObject;
    public Rigidbody rb;
    private GameObject Clone;
    public float FirstSpawn = 5f;
    public bool cont;

    void Start()
    {
        cont = true;
    }

    void FixedUpdate()
    {
        FirstSpawn -= Time.deltaTime;
        if (FirstSpawn <= 0f && cont)
        {
            Clone = Instantiate(CyclistObject, gameObject.transform.localPosition, Quaternion.Euler(0, 90, 0));

            // Get Rigidbody from children in case it's on a child object (e.g. OtterOnBike)
            Rigidbody cloneRb = Clone.GetComponentInChildren<Rigidbody>();

            int rnd_velo = UnityEngine.Random.Range(10, 25);

            Clone.AddComponent<NPCStraight>();
            Clone.GetComponent<NPCStraight>().gb = cloneRb != null ? cloneRb.gameObject : Clone;
            Clone.GetComponent<NPCStraight>().velocity = rnd_velo;

            if (cloneRb != null)
                cloneRb.linearVelocity = new Vector3(rnd_velo, 0, 0);

            float timeToSpawn = UnityEngine.Random.Range(10f, 15f);
            FirstSpawn = timeToSpawn;
        }
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