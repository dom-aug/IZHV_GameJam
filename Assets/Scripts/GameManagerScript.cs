using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private LocationType locationType = LocationType.Country;
    private float locationChangeFrequencyMean = 10.0f;
    private float locationChangeFrequencyStd = 5.0f;

    private enum LocationType
    {
        Country,
        Town
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
