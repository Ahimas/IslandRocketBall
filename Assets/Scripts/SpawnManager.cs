using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9.0f;

    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefab;
    public GameObject[] miniEnemyPrefab;
    public GameObject bossPrefab;
        
    public int enemyWave = 1;
    public int enemiesCurrent;
    public int bossWave = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy(enemyWave);
        SpawnPowerup();

    }

    // Update is called once per frame
    void Update()
    {
        enemiesCurrent = FindObjectsOfType<Enemy>().Length;

        if ( enemiesCurrent == 0 )
        {
            enemyWave++;

            if ( enemyWave % bossWave == 0 )
            {
                SpawnBoss();

            } else
            {
                SpawnEnemy(enemyWave);

            }
                        
            SpawnPowerup();

        }
    }
    void SpawnEnemy(int enemyWave)
    {
        for ( int i = 0; i < enemyWave; i++ )
        {
            int randomPrefab = Random.Range(0, enemyPrefab.Length);

            Instantiate(enemyPrefab[randomPrefab], GenerateSpawnPosition(), enemyPrefab[randomPrefab].transform.rotation);
        }
    }

    void SpawnBoss()
    {
        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);

        boss.GetComponent<Enemy>().miniEnemiesQnt = 1 + (enemyWave / bossWave);
    }

    public void SpawnMiniEnemies(int qnt)
    {
        for ( int i = 0; i < qnt; i++ )
        {
            int randomPrefab = Random.Range(0, miniEnemyPrefab.Length);

            Instantiate(miniEnemyPrefab[randomPrefab], GenerateSpawnPosition(), miniEnemyPrefab[randomPrefab].transform.rotation);
        }
    }

    void SpawnPowerup()
    {
        int randomPrefab = Random.Range(0, powerupPrefab.Length);

        Instantiate(powerupPrefab[randomPrefab], GenerateSpawnPosition(), powerupPrefab[randomPrefab].transform.rotation);
    }


    private Vector3 GenerateSpawnPosition()
    {
        float posX = Random.Range(-spawnRange, spawnRange);
        float posZ = Random.Range(-spawnRange, spawnRange);

        return new Vector3(posX, 0.4f, posZ);
    }
}
