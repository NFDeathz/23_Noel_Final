using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnScript : MonoBehaviour
{
    public GameObject EnemyRemaining;
    public GameObject EnemyPrefab;
    public float spawnInterval;
    public float maxEnemies;


    //spawn area
    public float minX; // minX position
    public float maxX; // maxX position
    public float minZ; // minZ position
    public float maxZ; // maxZ position

    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(WaitAndSpawn(spawnInterval));
    }

    // Update is called once per frame
    void Update()
    {

        if (maxEnemies == 0)
        {
            maxEnemies = 0;
            StopAllCoroutines();
        }     
    }

    private IEnumerator WaitAndSpawn(float waitTime)
    {
        if (maxEnemies != 0)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);

                Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));

                Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

                maxEnemies -= 1;
            }
        }  
    }
}
