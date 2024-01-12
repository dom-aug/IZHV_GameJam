using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{

    public float objectsSpeed = 10.0f;
    public LayerMask spawnLayer;
    public List<GameObject> roadPrefabs;
    public List<GameObject> surroundingsPrefabs;

    private float spawnAccumulator = 0.0f;

    [Header("centres of generation points")]
    public float roadXCenter = 10.0f;
    public float leftSurroundingsXCenter = 20.0f;
    public float rightSurroundingsXCenter = -20.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAccumulator >= objectsSpeed)
        {
            spawnTrio();
            spawnAccumulator = 0.0f;
        } else {
            spawnAccumulator += 1;
        }
    }

    public void spawnTrio()
    {
        spawnRoad();
        spawnSurroundings();
    }

    public void spawnRoad()
    {
        var roadPrefab = roadPrefabs[Random.Range(0, roadPrefabs.Count)];
        
        GameObject newObject = Instantiate(roadPrefab);

        newObject.transform.position = new Vector3( transform.position.y, transform.position.y, transform.position.z);

        var xScale = 10 / newObject.GetComponent<BoxCollider>().size.x;
        newObject.transform.localScale = new Vector3(xScale, 1, 1);
        newObject.AddComponent<EnvironmentObjectScript>();
    }

    public void spawnSurroundings()
    {
        var surroundingsPrefab = surroundingsPrefabs[Random.Range(0, surroundingsPrefabs.Count)];
        spawnObject(surroundingsPrefab, leftSurroundingsXCenter);

        surroundingsPrefab = surroundingsPrefabs[Random.Range(0, surroundingsPrefabs.Count)];
        spawnObject(surroundingsPrefab, rightSurroundingsXCenter);
    }

    private void spawnObject(GameObject gameObject, float XlocationCenter) {
        GameObject newObject = Instantiate(gameObject);

        var spawnPositionX = Random.value * 10.0f;
        var positionX = XlocationCenter > 0 ? spawnPositionX + XlocationCenter : spawnPositionX - XlocationCenter;
        
        newObject.transform.position = new Vector3(positionX, transform.position.y-0.01f, transform.position.z);
        newObject.transform.localScale = new Vector3(1, 1, 1);
        newObject.AddComponent<EnvironmentObjectScript>();
    }
}