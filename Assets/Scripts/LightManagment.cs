/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains methods for managing light and lamps in the apartment.
/// </summary>
public class LightManagment : MonoBehaviour
{
    private readonly string floorName = "floor";
    private Dictionary<string, float> spotLightsIntensity;
    private Dictionary<string, float> pointLightsIntensity;
    private GameObject originalLight;


    public LightManagment()
    {
        this.spotLightsIntensity = new Dictionary<string, float>();
        this.pointLightsIntensity = new Dictionary<string, float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.originalLight = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Lamp", MainConfig.Theme)[0]) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            string colliderName = collision.gameObject.name;

            if (colliderName.Contains(floorName))
            {
                SwitchLights(collision);
            }
        }
    }

    private void SwitchLights(Collision collision)
    {
        foreach (Light light in collision.gameObject.transform.GetComponentsInChildren<Light>())
        {
            switch (light.type)
            {
                case LightType.Point:
                    SwitchPointLight(light);
                    break;
                case LightType.Spot:
                    SwitchSpotLight(light);
                    break;
                default:
                    break;
            }
        }
    }

    private void SwitchPointLight(Light light)
    {
        if (this.pointLightsIntensity.ContainsKey(light.name))
        {
            float intensity = this.pointLightsIntensity[light.name];
            light.intensity = light.intensity == 0f ? intensity : 0f;
        }
        else
        {
            this.pointLightsIntensity[light.name] = this.originalLight.GetComponentsInChildren<Light>()[0].intensity;
        }
    }

    private void SwitchSpotLight(Light light)
    {
        if (this.spotLightsIntensity.ContainsKey(light.name))
        {
            float intensity = this.spotLightsIntensity[light.name];
            light.intensity = light.intensity == 0f ? intensity : 0f;
        }
        else
        {
            this.spotLightsIntensity[light.name] = this.originalLight.GetComponentsInChildren<Light>()[1].intensity;
        }
    }
}
