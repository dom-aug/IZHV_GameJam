using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleScript : MonoBehaviour
{

    public Light sun;

    [Range(0, 24)] public float timeOfDay;

    public float sunRotationSpeed = 0.1f;

    [Header("Lightning preset")]
    public Gradient skyColor;
    public Gradient equatorColor;
    public Gradient sunColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /* void Update()
    {
        
    } */


    private void updateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z));
    }

    private void updateLightning()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }

    private void OnValidate() {
        updateSunRotation();
        updateLightning();
    }

    private void Update()
    {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        timeOfDay %= 24;
        updateSunRotation();
        updateLightning();
    }
}
