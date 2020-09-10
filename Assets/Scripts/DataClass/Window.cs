/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Window
{
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
    private GameObject windowPrefab;

    public static List<BoxCollider> windowsCollider; //Contains the colliders of all doors in order to remove them later.


    private static readonly GameObject curtainPrefab;
    private static readonly Material curtainMaterial;
    private static readonly float windowBreakpoint;

    static Window()
    {
        windowBreakpoint = 1f;

        curtainMaterial = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Curtain", MainConfig.Theme, ".mat")[0], typeof(Material)) as Material;
        curtainPrefab = Resources.Load("UtilPrefabs/curtainWithRodPrefab") as GameObject;
        UpdateSkinnedMeshRenderer();

        windowsCollider = new List<BoxCollider>();
    }

    /// <summary>
    /// Extrude the window on the middle of the wall
    /// </summary>
    /// <param name="plan">Apartment plan</param>
    public void Extrude(Plan plan)
    {
        windowPrefab = LoadWindowPrefab();
        GameObject window = ExtrusionUtils.InstanciateComponent(windowPrefab, new Vector3(start[0], start[1]), new Vector3(stop[0], stop[1]), plan.wallHeight - plan.windowH1 - plan.windowH2, "Window", "window", 9); ;
        window.transform.position += new Vector3(0, plan.windowH1, 0);
        window.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);

        Vector3 windowPos = window.transform.position;

        BoxCollider boxCollider = window.AddComponent<BoxCollider>();
        windowsCollider.Add(boxCollider);
        boxCollider.size = new Vector3(1, 1, 2);
        boxCollider.center = new Vector3(0, plan.windowH1 / 2, 0);

        GameObject curtain = ExtrusionUtils.InstanciateComponent(curtainPrefab, new Vector3(start[0], start[1]), new Vector3(stop[0], stop[1]), plan.wallHeight, "Window", "curtain", 9);
        curtain.transform.position += new Vector3(0, plan.windowH1 + (plan.windowH2 - plan.windowH1) / 2 + 0.6f, 0f);
        curtain.transform.localScale += new Vector3(0, -0.6f, 0);

        float wallOffset = 0f;
        if(Math.Abs(stop[0] - start[0]) == 0)
        {
            wallOffset = 0.2f + 0.4f * Math.Abs(stop[1] - start[1]);
        }
        else if(Math.Abs(stop[1] - start[1]) == 0)
        {
            wallOffset =  0.2f + 0.4f * Math.Abs(stop[0] - start[0]);
        }

        PlaceCurtains(curtain, plan, wallOffset);
    }

    /// <summary>
    /// Position the curtains in the area
    /// </summary>
    /// <param name="curtain">Curtain GameObject</param>
    /// <param name="plan">Apartment plan</param>
    /// <param name="wallOffset">Wall offset to place the curtain</param>
    private void PlaceCurtains(GameObject curtain, Plan plan, float wallOffset)
    {

        foreach (Area area in plan.areas)
        {
            // face sud et nord
            // [0] x et [1] z
            Tuple<Vector2, Vector2> minMaxPoints = area.GetMinMaxPoints();

            float minX = minMaxPoints.Item1.x;
            float minY = minMaxPoints.Item1.y;

            float maxX = minMaxPoints.Item2.x;
            float maxY = minMaxPoints.Item2.y;

            if (start[0] >= minX && start[0] <= maxX && start[1] >= minY && start[1] <= maxY)
            {
                float meanX = (minX + maxX) / 2;
                float meanY = (minY + maxY) / 2;

                if (start[0] == stop[0])
                {
                    float dWindow = (stop[1] - start[1]) / 2;

                    if (meanX >= start[0])
                    {
                        curtain.transform.Rotate(new Vector3(0, 270, 0));
                        curtain.transform.position += new Vector3(wallOffset, 0, dWindow / 2);
                    }
                    else
                    {
                        curtain.transform.Rotate(new Vector3(0, 90, 0));
                        curtain.transform.position -= new Vector3(wallOffset, 0, dWindow / 2);
                    }
                    curtain.transform.localScale += new Vector3(0, 0, dWindow);
                }
                else if (start[1] == stop[1])
                {
                    float dWindow = (stop[0] - start[0]) / 2;

                    if (meanY >= start[1])
                    {
                        curtain.transform.Rotate(new Vector3(0, 270, 0));
                        curtain.transform.position += new Vector3(-dWindow /2, 0, wallOffset);
                    }
                    else
                    {
                        curtain.transform.Rotate(new Vector3(0, 90, 0));
                        curtain.transform.position -= new Vector3(-dWindow / 2, 0, wallOffset);
                    }
                    curtain.transform.localScale += new Vector3(0, 0, dWindow);
                }
            }
        }
    }

    /// <summary>
    /// Instatiate a window depending on the size
    /// </summary>
    /// <param name="plan">House plan</param>
    private GameObject LoadWindowPrefab()
    {
        GameObject windowPrefab = Resources.Load("UtilPrefabs/WindowPrefab") as GameObject;
        float dx = stop[0] - start[0];
        float dy = stop[1] - start[1];

        if (dx > windowBreakpoint || dy > windowBreakpoint)
            windowPrefab = Resources.Load("UtilPrefabs/BigWindowPrefab") as GameObject;

        return windowPrefab;
    }

    /// <summary>
    /// Updates the skinned mesh renderer
    /// </summary>
    private static void UpdateSkinnedMeshRenderer()
    {
        for (int i = 1; i < 3; i++)
        {
            Material[] mat = curtainPrefab.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterials;
            mat[0] = curtainMaterial;
            mat[1] = curtainMaterial;
            curtainPrefab.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = mat;
        }

    }
}
