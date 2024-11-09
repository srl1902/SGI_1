using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_luz : MonoBehaviour
{
    public Slider intensidad_slider;
    public Light[] pointLights;
    // Start is called before the first frame update
    void Start()
    {
        
        if(intensidad_slider != null)
        {
            intensidad_slider.onValueChanged.AddListener(UpdateIntensity);
        }
    }

    void UpdateIntensity(float value) 
    {
        foreach(Light light in pointLights)
        {
            if(light != null && light.type == LightType.Point)
            {
                light.intensity = value;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
