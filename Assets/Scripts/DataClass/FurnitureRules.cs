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
public class FurnitureRules
{
    public List<AreaRules> areaRules { get; set; }

    public List<FurnitureConstraints> furnitureConstraints { get; set; }

    /// <summary>
    /// Return the list of allow furniture type from a area type
    /// </summary>
    /// <param name="areaType">type of the area</param>
    /// <returns>Return the list of furniture names that may appear in this area</returns>
    public List<string> GetFurnitureTypeListByAreaType(AreaType areaType)
    {
        List<string> furnitureType = new List<string>();
        foreach(AreaRules rule in areaRules)
        {
            if (rule.type == areaType.ToString())
            {
                furnitureType = rule.furnitureType;
                break;
            }
        }
        return furnitureType;
    }

    /// <summary>
    /// Return the number of a given furniture in an area
    /// </summary>
    /// <param name="furnitureName">Furniture type name</param>
    /// <returns>Number of furniture in the area</returns>
    public int GetFurnitureQuantity(string furnitureName)
    {
        foreach (FurnitureConstraints fc in furnitureConstraints)
        {
            if(fc.furnitureName == furnitureName)
            {
                return fc.GetFurnitureQuantity();
            }
        }
        return 0;
    }
}
