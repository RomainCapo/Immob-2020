/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum AreaType
{
    [EnumMember(Value = "BathRoom")]
    Bathroom = 0,

    [EnumMember(Value = "Room")]
    Room = 1,

    [EnumMember(Value = "Kitchen")]
    Kitchen = 2,

    [EnumMember(Value = "LivingRoom")]
    LivingRoom = 3,

    [EnumMember(Value = "Corridor")]
    Corridor = 4
}
