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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cont = true;
        //StopSpawn.AddListener(StopSpawning);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FirstSpawn -= Time.deltaTime;
        if (FirstSpawn <= 0f && cont)
        {
            Clone = Instantiate(CyclistObject, gameObject.transform.localPosition, new Quaternion(0, 180f, 0, 1)) as GameObject;
            Rigidbody cloneRb = Clone.GetComponent<Rigidbody>();
            //cloneRb.AddForce(transform.right*10, ForceMode.VelocityChange);
            int rnd_velo = UnityEngine.Random.Range(10, 25);
            Clone.AddComponent<NPCStraight>();
            Clone.GetComponent<NPCStraight>().gb = Clone.gameObject;
            Clone.GetComponent<NPCStraight>().velocity = rnd_velo;
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
