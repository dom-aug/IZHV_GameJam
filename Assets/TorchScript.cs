using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TorchScript : MonoBehaviour
{
    private Light torchLight;
    private float torchLife = 100.0f;

    private ProgressBar TorchLifeProgressBarReference;


    // Start is called before the first frame update
    void Start()
    {
        torchLight = gameObject.GetComponent<Light>();

         var uiGameObject = GameObject.Find("UI");
        TorchLifeProgressBarReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("TorchLife");
    }

    // Update is called once per frame
    void Update()
    {
        if (torchLight.enabled) {
            torchLife -= Time.deltaTime * GameManagerScript.Instance.gameSpeed / 5.0f;

            if (torchLife <= 0.0f) {
                torchLight.enabled = false;
            }
        } else {
            if (torchLife <= 100.0f) {
                torchLife += 0.01f;
            }
        }

        TorchLifeProgressBarReference.value = torchLife;
        TorchLifeProgressBarReference.visible = torchLight.enabled || torchLife < 100.0f;


        /* var time = GameObject.Find("Directional Light").GetComponent<DayNightCycleScript>().timeOfDay;
        if (6.0f < time && time < 6.25f) {
            torchLife = 100.0f;
        } */
    }
}
