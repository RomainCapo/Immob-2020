/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

/// <summary>
/// Util class for furniture placement, wall extrusion and sykbox manage
/// </summary>
public static class MainConfig
{
    private static string theme = "Asian";
    private static string plan = "Assets/Resources/JSON/appart_default.json";
    private static string mode = "nightMat";
    public const string FURNITURE_RULES_PATH = "JSON/furnitureRules";

    /// <summary>
    /// Static value who contains the theme choosen by the user
    /// </summary>
    public static string Theme
    {
        get
        {
            return theme;
        }
        set
        {
            theme = value;
        }
    }

    /// <summary>
    /// Static value who contains the plan choosen by the user
    /// </summary>
    public static string Plan
    {
        get
        {
            return plan;
        }
        set
        {
            plan = value;
        }
    }

    /// <summary>
    /// Static value who contains the mode (day/night) choose by the user
    /// </summary>
    public static string Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
        }
    }
}
