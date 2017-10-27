using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnItems;
    public float spawnTimer = 5f;
    public float spawnTimerMin = 2f;

    private float currentSpawnTimer;
    private float currentSpawnTimerMax;
    
    public void Start()
    {
        currentSpawnTimerMax = Random.Range(spawnTimerMin, spawnTimer);
    }

    public void Update()
    {
        currentSpawnTimer += Time.deltaTime;
        if (currentSpawnTimer >= currentSpawnTimerMax)
        {
            currentSpawnTimer = 0f;
            currentSpawnTimerMax = Random.Range(spawnTimerMin, spawnTimer);
            GameObject spawn = Instantiate(spawnItems[Random.Range(0, spawnItems.Length)], transform.position, Quaternion.identity);
        }
    }
}
