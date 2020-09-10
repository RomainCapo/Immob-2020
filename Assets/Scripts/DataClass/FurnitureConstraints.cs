/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class FurnitureConstraints
{ 
    public enum QuantityConstraint
    {
        [EnumMember(Value = "ZeroToOne")]
        ZeroToOne,

        [EnumMember(Value = "OneToOne")]
        OneToOne,

        [EnumMember(Value = "ZeroToMany")]
        ZeroToMany,

        [EnumMember(Value = "OneToMany")]
        OneToMany,
    }

    public const int MAX_FURNTITURE_ALLOWED_BY_ROOM = 3;

    public string furnitureName { get; set; }

    public QuantityConstraint quantityConstraints { get; set; }

    /// <summary>
    /// Get the specific furniture quantity for one room
    /// </summary>
    /// <returns>Number of furniture for a room</returns>
    public int GetFurnitureQuantity()
    {
        int quantity = 0;
        switch (quantityConstraints)
        {
            case QuantityConstraint.ZeroToOne:
                quantity =  Random.Range(0, 2);
                break;

            case QuantityConstraint.OneToOne:
                quantity = 1;
                break;

            case QuantityConstraint.ZeroToMany:
                quantity = Random.Range(0, MAX_FURNTITURE_ALLOWED_BY_ROOM + 1);
                break;

            case QuantityConstraint.OneToMany:
                quantity = Random.Range(1, MAX_FURNTITURE_ALLOWED_BY_ROOM);
                break;

            default:
                quantity = 0;
                break;
        }
        return quantity;
    }
}
