/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[System.Serializable]
public class Area
{
    public AreaType type { get; set; }
    public List<List<float>> points { get; set; }
    public Boolean isLightOn { get; set; }

    private const float ceilingThickness = 0.1f;

    private static readonly GameObject ceilingLampPrefab;
    private static readonly Material floorMaterial;
    private static readonly GameObject apartment;
    private static readonly GameObject curtain;

    static Area()
    {
        string currentTheme = MainConfig.Theme;
        ceilingLampPrefab = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Lamp", currentTheme)[0]) as GameObject;
        floorMaterial = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Floor", currentTheme, ".mat")[0], typeof(Material)) as Material;
        apartment = GameObject.FindGameObjectWithTag("Apartment");
        curtain = Resources.Load("curtains") as GameObject;
    }

    /// <summary>
    /// Instanciate the lights, the lamp prefab and places them in the middle of the room
    /// </summary>
    /// <param name="plan">Apartment plan</param>
    /// <returns>return the current lamp of the area</returns>
    public GameObject InstanciateLights(Plan plan)
    {
        List<float> geoCenter = points[0].Select((dummy, i) => points.Average(inner => inner[i])).ToList();
        Vector3 v3GeoCenter = new Vector3(geoCenter[0], plan.wallHeight, geoCenter[1]);

        GameObject ceilingLamp = GameObject.Instantiate(ceilingLampPrefab) as GameObject;
        foreach (Light light in ceilingLamp.transform.GetComponentsInChildren<Light>())
        {
            light.intensity = isLightOn ? light.intensity : 0;
        }

        ceilingLamp.transform.position = v3GeoCenter;
        ceilingLamp.name = type + "__light";

        return ceilingLamp;
    }
    
    /// <summary>
    /// Instanciate the ceilling and the floor of an area and scale the floor texture
    /// </summary>
    /// <param name="plan">Apartment plan</param>
    /// <returns>return the floor object to the current area</returns>
    public GameObject InstanciateCeilingAndFloor(Plan plan)
    {
        List<float> geoCenter = points[0].Select((dummy, i) => points.Average(inner => inner[i])).ToList();
        Vector3 v3GeoCenter = new Vector3(geoCenter[0], plan.wallHeight, geoCenter[1]);

        Tuple<Vector2, Vector2> minMaxPoints = GetMinMaxPoints();

        Vector3 scale = new Vector3(minMaxPoints.Item2.x- minMaxPoints.Item1.x, ceilingThickness, minMaxPoints.Item2.y - minMaxPoints.Item1.y);

        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.transform.position = v3GeoCenter;
        ceiling.transform.localScale = scale;
        ceiling.transform.parent = apartment.transform;
        ceiling.name = type + "_ceiling";

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        v3GeoCenter.y = 0;
        floor.transform.position = v3GeoCenter;
        floor.transform.position += new Vector3(0, -ceilingThickness /2, 0);
        floor.transform.localScale = scale;
        floor.transform.parent = apartment.transform;
        floor.GetComponent<MeshRenderer>().material = floorMaterial;
        floor.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(scale.x, scale.z);
        floor.name = type + "_floor";
        floor.layer = LayerMask.NameToLayer("Floor");

        return floor;
    }


    /// <summary>
    /// Return the X and Z minimum and maximum point of an area
    /// </summary>
    /// <returns>Tuple of PointF that contain the minimum and the maximum point</returns>
    public Tuple<Vector2, Vector2> GetMinMaxPoints()
    {
        List<float> minPoints = this.points.SelectMany(x => x.Select((v, i) => new { v, i }))
                 .GroupBy(x => x.i, x => x.v)
                 .OrderBy(g => g.Key)
                 .Select(g => g.Min())
                 .ToList();
        float minX = minPoints[0];
        float minY = minPoints[1];

        List<float> maxPoints = this.points.SelectMany(x => x.Select((v, i) => new { v, i }))
             .GroupBy(x => x.i, x => x.v)
             .OrderBy(g => g.Key)
             .Select(g => g.Max())
             .ToList();

        float maxX = maxPoints[0];
        float maxY = maxPoints[1];

        return new Tuple<Vector2, Vector2>(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }
}
