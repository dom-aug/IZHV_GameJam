using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSurroundingSpawner : MonoBehaviour
{
    public float initXminCoordinate = -10.0f;
    public float initXmaxCoordinate = 10.0f;
    public float initZminCoordinate = -10.0f;
    public float initZmaxCoordinate = 10.0f;

    public List<GameObject> surroundingsPrefabs;

    public LayerMask spawnLayer;

    public int initSurroundingsObjectCount = 10;

    private bool surroundingsSpawned = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (surroundingsSpawned)
        {
            return;
        }

        if (GameManagerScript.Instance.gameStarted == false)
        {
            return;
        }

        if (GameManagerScript.Instance.gameStarted == true)
        {
            for (int i = 0; i < initSurroundingsObjectCount; i++)
            {
                spawnSurroundings();
            }
            surroundingsSpawned = true;
        }
    }

    private void spawnSurroundings()
    {
        int index = Random.Range(0, surroundingsPrefabs.Count);
        var surroundingsPrefab = surroundingsPrefabs[index];

        var xCoordinate = Random.Range(initXminCoordinate, initXmaxCoordinate);
        var zCoordinate = Random.Range(initZminCoordinate, initZmaxCoordinate);

        var spawnPosition = new Vector3(xCoordinate, 0.0f, zCoordinate);

        if (Physics.Raycast(spawnPosition, Vector3.back, 6f, spawnLayer))
        {
            return;
        }


        var surroundingsObject = Instantiate(surroundingsPrefab, spawnPosition, Quaternion.identity);
        surroundingsObject.transform.parent = transform;
        
        if (index != 0)
        {
            surroundingsObject.transform.Rotate(90.0f, 0.0f, 0.0f);
        }

        surroundingsObject.layer = LayerMask.NameToLayer("Obstacle");

        surroundingsObject.AddComponent<EnvironmentObjectScript>();

        var scale = Random.Range(0.8f, 1.2f);
        surroundingsObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}
