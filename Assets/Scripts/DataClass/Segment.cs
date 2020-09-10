/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment
{
    public string name { get; set; }
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
    public Window window { get; set; }

    public static readonly GameObject wallPrefab;

    static Segment()
    {
        wallPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Material wallMaterial = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Wall", MainConfig.Theme, ".mat")[0], typeof(Material)) as Material;
        wallPrefab.GetComponent<MeshRenderer>().material = wallMaterial;
    }

    /// <summary>
    /// Extrude the wall of the apartment, Build a single wall if there is no window, otherwise build 4 walls to place the window and create the window
    /// </summary>
    /// <param name="plan">Plan apartment</param>
    public void Extrude(Plan plan)
    {
        if(window.start == null)
        {
            GameObject wall = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(start[0], start[1]), new Vector3(stop[0], stop[1]), plan.wallHeight,"Wall", name, 10);
            wall.transform.position += new Vector3(0, plan.wallHeight/2, 0);
            wall.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(start[0], start[1]), plan.wallHeight, plan.wallWidth, name + "_edge_left");
            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(stop[0], stop[1]), plan.wallHeight, plan.wallWidth, name + "_edge_right");
        }
        else
        {
            GameObject wallLeft = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(start[0], start[1]), new Vector3(window.start[0], window.start[1]), plan.wallHeight, "Wall", name + "_left", 10);
            wallLeft.transform.position += new Vector3(0, plan.wallHeight / 2, 0);
            wallLeft.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

            GameObject wallRight = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(window.stop[0], window.stop[1]), new Vector3(stop[0], stop[1]),plan.wallHeight, "Wall", name + "_right", 10);
            wallRight.transform.position += new Vector3(0, plan.wallHeight / 2, 0);
            wallRight.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

            GameObject wallUp = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(window.start[0], window.start[1]), new Vector3(window.stop[0], window.stop[1]),plan.windowH2, "Wall", name + "_up", 10);
            wallUp.transform.position += new Vector3(0, plan.wallHeight - (plan.windowH2/2), 0);
            wallUp.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

            GameObject wallDown = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(window.start[0], window.start[1]), new Vector3(window.stop[0], window.stop[1]), plan.windowH1, "Wall", name + "_down", 10);
            wallDown.transform.position += new Vector3(0, plan.windowH1/2, 0);
            wallDown.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(start[0], start[1]), plan.wallHeight, plan.wallWidth, name + "_edge_left");
            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(window.start[0], window.start[1]), plan.wallHeight, plan.wallWidth, name + "_edge_center_left");

            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(window.stop[0], window.stop[1]), plan.wallHeight, plan.wallWidth, name + "_edge_center_right");
            ExtrusionUtils.InstanciateEdge(wallPrefab, new Vector3(stop[0], stop[1]), plan.wallHeight, plan.wallWidth, name + "_edge_right");

            window.Extrude(plan);
        }
    }
}
