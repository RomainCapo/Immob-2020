/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class contains the methods for carrying out the placement of furniture in a given plan and placement rules.
/// </summary>
public class FurniturePlacement
{

    private Plan plan;
    private FurnitureRules furnitureRules;
    private String furnitureStyle;
    private GameObject apartment;
    
    private const int MAX_FURNITURE_PLACEMENT_ITERATION = 30;
    private const float AGAINST_WALL_COLLISION_OFFSET = 0.01f;

    private static readonly int MASK;

    static FurniturePlacement()
    {
        //Compute the collider mask for the Physics.OverlapsBox() function
        int wall = 1 << LayerMask.NameToLayer("Wall");
        int door = 1 << LayerMask.NameToLayer("Window");
        int window = 1 << LayerMask.NameToLayer("Door");
        int furniture = 1 << LayerMask.NameToLayer("Furniture");

        MASK = wall | door | window | furniture;
    }

    public FurniturePlacement(Plan plan, FurnitureRules fr, GameObject apartment)
    {
        this.plan = plan;
        furnitureRules = fr;
        furnitureStyle = MainConfig.Theme;
        this.apartment = apartment;
    }

    /// <summary>
    /// Process the furniture placement in all area in the given plan passed in the constructor
    /// </summary>
    public void Process()
    {
        // Loop throught all area type
        foreach(Area area in plan.areas)
        {
            Tuple<Vector2, Vector2> minMaxPoints = area.GetMinMaxPoints();

            // Loop throught all furniture available for an area type
            foreach (string furnitureType in furnitureRules.GetFurnitureTypeListByAreaType(area.type))
            {
                // Loop for number of furniture to dispose in the area
                for (int j = 0; j < furnitureRules.GetFurnitureQuantity(furnitureType); j++)
                {
                    bool isFurniturePlacementCorrect = false;
                    int i = 0;
                    do
                    {
                        List<string> listFurnitureType = FolderUtils.GetFurnitureNamesFromFurnitureType(furnitureType, furnitureStyle);

                        int randomFurnitureType = Random.Range(0, listFurnitureType.Count);
                        GameObject currentFurniture = GameObject.Instantiate(Resources.Load(listFurnitureType[randomFurnitureType]) as GameObject);
                        currentFurniture.transform.localScale *= currentFurniture.GetComponent<FurnitureProperties>().GetScale();
                        currentFurniture.tag = "Furniture";
                        currentFurniture.layer = LayerMask.NameToLayer("Furniture");
                        currentFurniture.name = currentFurniture.name + "-" + j;
                        currentFurniture.transform.parent = apartment.transform;

                        AddColliderToGameObject(currentFurniture);
                        bool isVertical = ComputeObjectPosition(currentFurniture, minMaxPoints);

                        isFurniturePlacementCorrect = IsFurniturePlacementCorrect(currentFurniture, isVertical);

                        if (isFurniturePlacementCorrect)
                        {
                            currentFurniture.transform.position = new Vector3(200, 200, 200); // Allows the furniture to be moved far into the scene so that it does not collide with other furniture
                            GameObject.Destroy(currentFurniture);
                        }

                        i++;
                    } while (isFurniturePlacementCorrect && i <= MAX_FURNITURE_PLACEMENT_ITERATION); // Loop while the furniture is collided or if the counter not reach the maximum iteration allowed
                }
            }
        }     
    }

    /// <summary>
    /// Compute the object rotation based on the logest wall of the room.
    /// The room is cut in half from the longest wall of the room. The function then detects whether the furniture is 
    /// in the upper or lower part of the room and adjusts the rotation of the furniture accordingly.
    /// </summary>
    /// <param name="furniture">GameObject to compute the rotation</param>
    /// <param name="minMaxPoints">Tuple that contain the min and the max point of the room</param>
    /// <returns>return true if the furniture is positioned vertically, false otherwise</returns>
    private bool ComputeObjectPosition(GameObject furniture, Tuple<Vector2, Vector2> minMaxPoints)
    {
        BoxCollider bc = furniture.GetComponent<BoxCollider>();
        Vector3 furnitureSize = bc.size;

        //Places the furniture at a random location in the room
        float randX = Random.Range(minMaxPoints.Item1.x + plan.wallWidth + (furnitureSize.x/2) , minMaxPoints.Item2.x - plan.wallWidth - (furnitureSize.x / 2));
        float randZ = Random.Range(minMaxPoints.Item1.y + plan.wallWidth + (furnitureSize.z/2), minMaxPoints.Item2.y - plan.wallWidth - (furnitureSize.z / 2));
        furniture.transform.position = new Vector3(randX, furniture.transform.position.y, randZ);

        float xOffset = minMaxPoints.Item1.x;
        float zOffset = minMaxPoints.Item1.y;

        //Compute the height and width of the area
        float xSize = Math.Abs(minMaxPoints.Item1.x - minMaxPoints.Item2.x);
        float zSize = Math.Abs(minMaxPoints.Item1.y - minMaxPoints.Item2.y);

        float middle = 0.0f;
        float finalAngle = 0.0f;
        bool isVertical = true;

        FurnitureProperties.PlacementType fp = furniture.GetComponent<FurnitureProperties>().placementType;

        //Checks whether the distance of the area is greater on the X or Z axis. This will allow the furniture to be oriented on the longest side of the room.
        if (xSize > zSize)
        {
            middle = zSize / 2 + zOffset;

            //Controls whether or not the wall is bigger than half.This will allow it to be oriented towards the centre of the area.
            if (furniture.transform.position.z > middle)
            {
                finalAngle = 180;

                if (fp == FurnitureProperties.PlacementType.AgainstTheWall)
                {
                    furniture.transform.position = new Vector3(furniture.transform.position.x, 
                        furniture.transform.position.y, 
                        minMaxPoints.Item2.y - (plan.wallWidth/2) - (bc.bounds.size.z / 2) + AGAINST_WALL_COLLISION_OFFSET);//If the furniture is to be glued to the wall, the Z-coordinate is placed to the wall.
                }
            }
            else
            {
                finalAngle = 0;

                if(fp == FurnitureProperties.PlacementType.AgainstTheWall)
                {
                    furniture.transform.position = new Vector3(furniture.transform.position.x, 
                        furniture.transform.position.y, 
                        minMaxPoints.Item1.y + (plan.wallWidth/2) + (bc.bounds.size.z / 2) - AGAINST_WALL_COLLISION_OFFSET);//If the furniture is to be glued to the wall, the Z-coordinate is placed to the wall.
                }
            }
            isVertical = true;
        }
        else
        {
            middle = (xSize / 2) + xOffset;

            //Controls whether or not the wall is bigger than half.This will allow it to be oriented towards the centre of the area.
            if (furniture.transform.position.x > middle)
            {
                finalAngle = 270;
                if (fp == FurnitureProperties.PlacementType.AgainstTheWall)
                {
                    furniture.transform.position = new Vector3(minMaxPoints.Item2.x - (plan.wallWidth / 2) - (bc.bounds.size.z / 2) + AGAINST_WALL_COLLISION_OFFSET, //If the furniture is to be glued to the wall, the X-coordinate is placed to the wall.
                                                                furniture.transform.position.y, 
                                                                furniture.transform.position.z);
                }
            }
            else
            {
                finalAngle = 90;

                if (fp == FurnitureProperties.PlacementType.AgainstTheWall)
                {
                    furniture.transform.position = new Vector3(minMaxPoints.Item1.x + (plan.wallWidth / 2) + (bc.bounds.size.z / 2) - AGAINST_WALL_COLLISION_OFFSET, //If the furniture is to be glued to the wall, the X-coordinate is placed to the wall.
                                                                furniture.transform.position.y, 
                                                                furniture.transform.position.z);
                }
            }
            isVertical = false;
        }
        furniture.transform.Rotate(0, finalAngle, 0);
        return isVertical;
    }

    /// <summary>
    /// For furniture that is to be placed against the wall, check that it is against a wall and does not collide with other objects in the room, 
    /// if it is to be placed everywhere check that it does not collide with other objects in the scene.
    /// </summary>
    /// <param name="furniture">Furniture to check</param>
    /// <param name="isVertical">true is the gameobject to test is vertical, false otherwise</param>
    /// <returns>return true if the furniture is placed correctly, false othwewise</returns>
    private bool IsFurniturePlacementCorrect(GameObject furniture, bool isVertical)
    {
        bool isFurniturePlacementCorrect = false;
        if (furniture.GetComponent<FurnitureProperties>().placementType == FurnitureProperties.PlacementType.AgainstTheWall)
        {
            isFurniturePlacementCorrect = !IsAgainstWall(furniture, isVertical);
        }
        else
        {
            isFurniturePlacementCorrect = isCollided(furniture);
        }
        return isFurniturePlacementCorrect;
    }

    /// <summary>
    /// Checks whether the furniture that is supposed to be placed against the wall is actually placed against the wall
    /// </summary>
    /// <param name="furniture">Gameobject to test</param>
    /// <param name="isVertical">true is the gameobject to test is vertical, false otherwise</param>
    /// <returns>return true if the gameobject is against the wall, false otherwise</returns>
    private bool IsAgainstWall(GameObject furniture, bool isVertical)
    {
        bool isAgainstWall = false;

        Physics.SyncTransforms();
        Collider[] intersecting = Physics.OverlapBox(furniture.transform.position, furniture.GetComponent<BoxCollider>().bounds.size / 2, Quaternion.Euler(0, furniture.transform.rotation.y, 0), MASK);//Get all the collision with the objects
        foreach (Collider collid in intersecting)
        {
            //Check if the furniture isn't in collision with himself and an edge wall
            if (!collid.name.Equals(furniture.name) && !collid.name.Contains("edge"))
            {
                if (collid.gameObject.layer == 10)//Check if the furniture is collided with a wall
                    {
                    if (isVertical)
                        {
                        //Checks that the furniture is completely against the wall and that it only collides with vertical walls
                        if (furniture.transform.position.x + (furniture.GetComponent<BoxCollider>().bounds.size.x / 2) < (collid.transform.position.x + (collid.transform.localScale.x / 2)) &&
                                furniture.transform.position.x - (furniture.GetComponent<BoxCollider>().bounds.size.x / 2) > (collid.transform.position.x - (collid.transform.localScale.x / 2)) &&
                                collid.transform.localEulerAngles.y == 0)
                            {
                                isAgainstWall = true;
                            }
                        }
                        else
                        {
                        //Checks that the furniture is completely against the wall and that it only collides with horizontal walls
                        if (furniture.transform.position.z + (furniture.GetComponent<BoxCollider>().bounds.size.z / 2) < (collid.transform.position.z + (collid.transform.localScale.x / 2)) &&
                                furniture.transform.position.z - (furniture.GetComponent<BoxCollider>().bounds.size.z / 2) > (collid.transform.position.z - (collid.transform.localScale.x / 2)) &&
                                collid.transform.localEulerAngles.y != 0)
                            {
                                isAgainstWall = true;
                            }
                        }
                    }
                    else //If the wall isn't on the wall, break the loop
                    {
                        isAgainstWall = false;
                        break;
                    }
            }
        }
        return isAgainstWall;
    }

    /// <summary>
    /// Check if the GameObject passed in parameters isCollider with a wall, a door or an other furniture
    /// </summary>
    /// <param name="gameObject">GameObject to test</param>
    /// <returns>true if the object is collided with a wall, a door or a furniture. false otherwise</returns>
    private bool isCollided(GameObject gameObject)
    {  
        bool isCollided = false;

        Physics.SyncTransforms();
        Collider[] intersecting = Physics.OverlapBox(gameObject.transform.position, gameObject.GetComponent<BoxCollider>().bounds.size / 2, Quaternion.Euler(0, gameObject.transform.rotation.y, 0), MASK);//Get all the collision with the objects
        foreach (Collider collid in intersecting)
        {
            if (!collid.name.Equals(gameObject.name))
            {
                isCollided = true;
            }
        }
        return isCollided;
    }

    /// <summary>
    /// Add a BoxCollider to a GameObject if he doesn't have one
    /// Check also if the collider has the default size value (1,1,1)
    /// If the collider is null or it has the dafault value, his collider will be calculated from his children
    /// </summary>
    /// <param name="obj">GameObject to add the collider</param>
    public static void AddColliderToGameObject(GameObject obj)
    {
        BoxCollider collider = obj.GetComponent<BoxCollider>();

        if(collider == null)
        {
            collider = obj.AddComponent<BoxCollider>();
            collider = ComputeNewCollider(obj);
        }

        if((collider.size.x == 1 && collider.size.y == 1 && collider.size.z == 1))
        {
            collider = ComputeNewCollider(obj);
        }
    }

    /// <summary>
    /// Compute the collider of the parent object from the children object
    /// Loop across the children and encapsulate their collider to get the collider of the parent.
    /// </summary>
    /// <param name="obj">GameObjet to compute new box collider</param>
    /// <returns>New BoxCollider object</returns>
    public static BoxCollider ComputeNewCollider(GameObject obj)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Transform child in obj.transform)
        {
            BoxCollider childCollider = child.GetComponent<BoxCollider>();
            if(childCollider != null)
            {
                bounds.Encapsulate(childCollider.bounds);
            }
        }

        //Create the new collider from the bounds
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        boxCollider.center = bounds.center;

        if(obj.transform.rotation.eulerAngles.y == 0 || obj.transform.rotation.eulerAngles.y == 180)
        {
            boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
        }
        else
        {
            boxCollider.size = new Vector3(bounds.size.z, bounds.size.y, bounds.size.x);
        }

        return boxCollider;
    }
}
