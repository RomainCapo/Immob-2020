/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JSONParser
{
    /// <summary>
    /// Load and parse the given JSON plan
    /// </summary>
    /// <param name="path">Plan path</param>
    /// <returns>Return Plan object with JSON informations inside</returns>
    public static Plan ParsePlan(string path)
    {
        return JsonConvert.DeserializeObject<Plan>(File.ReadAllText(path));
    }

    /// <summary>
    /// Load and parse the JSON furniture rules file
    /// </summary>
    /// <returns>Return FurnituresRules object with JSON informations inside</returns>
    public static FurnitureRules ParseFurnitureRules()
    {
        string json = (Resources.Load(MainConfig.FURNITURE_RULES_PATH) as TextAsset).text;
        return JsonConvert.DeserializeObject<FurnitureRules>(json);
    }
}