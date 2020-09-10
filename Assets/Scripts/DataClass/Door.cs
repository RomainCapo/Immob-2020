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
public class Door
{
    public string name { get; set; }
    public bool isFrontDoor { get; set; }
    public List<float> start { get; set; }
    public List<float> stop { get; set; }

    public static List<BoxCollider> doorsCollider; //Contains the colliders of all doors in order to remove them later.
    public static readonly GameObject wallPrefab;
    private static readonly GameObject doorPrefab;
    private const float DOOR_WIDTH_OFFSET = 0.1f;

    static Door()
    {
        string currentTheme = MainConfig.Theme;

        wallPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Material wallMaterial = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Wall", currentTheme, ".mat")[0], typeof(Material)) as Material;
        wallPrefab.GetComponent<MeshRenderer>().material = wallMaterial;

        doorPrefab = Resources.Load(FolderUtils.GetFurnitureNamesFromFurnitureType("Door", currentTheme)[0]) as GameObject;
        doorsCollider = new List<BoxCollider>();
    }

    /// <summary>
    /// Extrude the wall above the door, extrude the door and add the collider to the door, the collider is added for the furniture placement
    /// </summary>
    /// <param name="plan"></param>
    public void Extrude(Plan plan)
    {
        GameObject doorWall = ExtrusionUtils.InstanciateComponent(wallPrefab, new Vector3(start[0], start[1]), new Vector3(stop[0], stop[1]), plan.doorH2, "Door", name + "_up", 8);
        doorWall.transform.localScale += new Vector3(-plan.wallWidth, 0, plan.wallWidth);
        doorWall.transform.position += new Vector3(0, plan.wallHeight - (plan.doorH2 /2), 0);

        GameObject doorObject = ExtrusionUtils.InstanciateComponent(doorPrefab, new Vector3(start[0], start[1]), new Vector3(stop[0], stop[1]), 1, "Door", name, 8);
        float xOffset = Math.Abs(start[0] - stop[0]);
        float zOffset = Math.Abs(start[1] - stop[1]);
        if(xOffset == 0)
        {
            doorObject.transform.localScale += new Vector3(0, 0, zOffset-DOOR_WIDTH_OFFSET);
        }
        else if(zOffset == 0)
        {
            doorObject.transform.localScale += new Vector3(0, 0, xOffset- DOOR_WIDTH_OFFSET);
        }
        

        // Add the collider to doors
        BoxCollider boxCollider = doorObject.AddComponent<BoxCollider>();
        doorsCollider.Add(boxCollider);

        if (doorObject.transform.eulerAngles.y == 0)
        {
            boxCollider.size = new Vector3(1, plan.wallHeight, 1.5f);
            boxCollider.center = new Vector3(0, plan.wallHeight / 2, 0);
        }
        else
        {
            boxCollider.size = new Vector3(1.5f, plan.wallHeight, 1);
            boxCollider.center = new Vector3(0, plan.wallHeight / 2, 0);
        }

        //Allows to block the front door
        if (isFrontDoor)
        {
            GameObject.Destroy(doorObject.GetComponent<OpenDoor>());
            doorObject.layer = LayerMask.NameToLayer("FrontDoor");

            //Add layer to children object door
            foreach (Transform child in doorObject.transform)
            {
                if (child == null)
                {
                    continue;
                }
                child.gameObject.layer = 18;
            }
        }
    }
}
