/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using UnityEngine;

/// <summary>
/// This script must be placed on an object and is used to add metadata to it. It is used to indicate what scale the piece of furniture should have and where it should be placed.
/// </summary>
public class FurnitureProperties : MonoBehaviour
{
    public enum PlacementType
    {
        AgainstTheWall,
        Everywhere
    }

    public enum FurnitureScalePack
    {
        Default,
        Chinese,
        Contemporary,
        Retro
    }

    /// <summary>
    /// Return the object scale from the enumeration
    /// </summary>
    /// <returns>Scale of a furniture</returns>
    public float GetScale()
    {
        float scale = 1;
        switch (this.furnitureScalePack)
        {
            case FurnitureScalePack.Chinese:
                scale = 1;
                break;
            case FurnitureScalePack.Contemporary:
                scale = 0.75f;
                break;
            case FurnitureScalePack.Retro:
                scale = 1f;
                break;
            default:
                scale = 1;
                break;
        }
        return scale;
    }

    public FurnitureScalePack furnitureScalePack;
    public PlacementType placementType;
  
}
