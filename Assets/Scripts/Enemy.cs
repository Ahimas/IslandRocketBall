using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody enemyRb;
    private GameObject player;
    private SpawnManager spawnManager; 
    public bool isBoss;
    public float spawnInterval = 5f;
    public int miniEnemiesQnt;
    private float spawnTime;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if ( isBoss )
        {
            spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
            spawnTime = Time.time + spawnInterval;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Chase();

        if ( isBoss && Time.time > spawnTime )
        {
            spawnTime += spawnInterval;
            spawnManager.SpawnMiniEnemies(miniEnemiesQnt);
        }

        if ( transform.position.y < -10f )
        {
            Destroy(gameObject);
        }
    }

    void Chase()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);
    }
}
