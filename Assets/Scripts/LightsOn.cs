using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    // Keep track of lights
    private GameObject areaLight;
    private GameObject pointLight;
    public Material newSky;

    // Keep track of active light number
    public int lights;
    
    void Start()
    {
        lights = 0;
    }

    

    // Set lights active when player enters the trigger of a street light
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StreetLight")
        {
            areaLight = other.gameObject.transform.Find("Area Light").gameObject;
            pointLight = other.gameObject.transform.Find("Point Light").gameObject;

            if (!areaLight.activeSelf)
            {
                areaLight.SetActive(true);
                pointLight.SetActive(true);

                lights += 1;

                // Change skybox to beautiful sky if all lights are light up
                if (lights == 11)
                {
                    RenderSettings.skybox = newSky;
                }
            }
            
        }
    }
}
