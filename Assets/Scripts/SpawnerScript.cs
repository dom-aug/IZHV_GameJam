using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerScript : MonoBehaviour
{
    public float spawnFrequencyMean = 1.0f;
    public float spawnFrequencyStd = 0.5f;
    public float spawnSize = 1.0f;

    public float spawnPositionXCenter = 0.0f;
    public LayerMask spawnLayer;
    public List<GameObject> obstaclesPrefabs;
    private float spawnAccumulator = 0.0f;
    private float nextSpawnIn = 0.0f;

    private bool spawnObstacles = true;



    // Start is called before the first frame update
    void Start()
    {
        ResetSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnObstacles)
        { // Check if we should spawn.
            spawnAccumulator += Time.deltaTime;
            if (spawnAccumulator >= nextSpawnIn)
            {
                // Spawn at most one obstacle per frame.
                spawnAccumulator -= nextSpawnIn;
                nextSpawnIn = RandomNormal(spawnFrequencyMean, spawnFrequencyStd);
                
                SpawnObstacle();
            }
        }
    }

    public void ResetSpawn()
    {
        spawnAccumulator = 0.0f;
        nextSpawnIn = RandomNormal(spawnFrequencyMean, spawnFrequencyStd);
    }

    public static float RandomNormal(float mean, float std)
    {
        var v1 = 1.0f - UnityEngine.Random.value;
        var v2 = 1.0f - UnityEngine.Random.value;
        
        var standard = Math.Sqrt(-2.0f * Math.Log(v1)) * Math.Sin(2.0f * Math.PI * v2);
        
        return (float)(mean + std * standard);
    }

    public static bool RandomBool()
    {
        return UnityEngine.Random.value >= 0.5;
    }

    public void SpawnObstacle()
    {
        // Spawn the obstacle.
        var obstaclePrefab = obstaclesPrefabs[UnityEngine.Random.Range(0, obstaclesPrefabs.Count)];
        var obstacle = Instantiate(obstaclePrefab, transform);
        
        // Move it to the target location.
        var spawnPositionX = UnityEngine.Random.value;

        var positionX = spawnPositionX * 10.0f + spawnPositionXCenter - 5.0f + spawnSize / 2.0f;
        obstacle.transform.position = new Vector3(
            positionX,
            transform.position.y,
            transform.position.z
        );
        
        
        // Scale it.
        var tmpSpawnSize = RandomNormal(spawnSize, 0.2f);
        obstacle.transform.localScale = new Vector3(tmpSpawnSize, tmpSpawnSize, tmpSpawnSize);

        obstacle.AddComponent<EnvironmentObjectScript>();
        obstacle.AddComponent<BoxCollider>();
    }
}
