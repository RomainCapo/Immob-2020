/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class contains utility methods for file management, especially for cross-platform use.
/// </summary>
public class FolderUtils
{
    const string DEFAULT_FURNITURE_STYLE_NAME = "Default";
    const string PREFAB_FILENAME = "prefab.txt";
    const string FURNITURE_TYPE_FILENAME = "furnitureType.txt";
    const string THEME_FILENAME = "theme.txt";
    const string RESOURCES_FOLDER = "/Resources";

    /// <summary>
    /// Creates for each theme a file in .txt format that contains the names of the furniture folders. 
    /// Creates for each type of furniture a .txt file that contains the name of the prefabs available in that folder for that theme. 
    /// This makes it possible to export our program on all platforms so that it is not dependent on a particular file system.
    /// Each element are separeted by a comma.
    /// This script is code is only executed in editor mode.
    /// </summary>
    public static void InitFurnitureFolders()
    {
        if (Application.isEditor)
        {
            foreach(string theme in GetTextContentFromResources("theme"))
            {
                string furnitureTypeStr = "";
                foreach (string furnitureTypePath in Directory.GetDirectories(Application.dataPath + "/Resources/" + theme))
                {
                    string furnitureTypeName = Path.GetFileName(furnitureTypePath);
                    furnitureTypeStr += furnitureTypeName + ";";
                    File.WriteAllText(Application.dataPath + RESOURCES_FOLDER + "/" + theme + "/" + FURNITURE_TYPE_FILENAME, furnitureTypeStr);

                    DirectoryInfo furnitureStyleInfo = new DirectoryInfo(Application.dataPath + RESOURCES_FOLDER + "/" + theme + "/" + furnitureTypeName);
                    List<FileInfo> fileInfos = furnitureStyleInfo.GetFiles().OrderBy(p => p.CreationTime).ToList();

                    string prefabStr = "";
                    foreach(FileInfo file in fileInfos)
                    {
                        if(file.Extension != ".meta" && file.Name != PREFAB_FILENAME)
                        {
                            prefabStr += Path.GetFileNameWithoutExtension(file.Name) + ";";
                        }
                    }
                    File.WriteAllText(Application.dataPath + RESOURCES_FOLDER + "/" + theme + "/" + furnitureTypeName + "/" + PREFAB_FILENAME, prefabStr);
                }
            }
        }
    }

    /// <summary>
    /// Add each furniture theme in a .txt file. 
    /// This script is code is only executed in editor mode.
    /// This makes it possible to export our program on all platforms so that it is not dependent on a particular file system.
    /// Each element are separeted by a comma.
    /// This script is code is only executed in editor mode.
    /// </summary>
    public static void InitThemeFolder()
    {
        if (Application.isEditor)
        {
            string themeStr = "";

            // Get all theme to add it to the textmeshpro dropdown menu
            DirectoryInfo theme = new DirectoryInfo(Application.dataPath  + RESOURCES_FOLDER);
            DirectoryInfo[] directoriesTheme = theme.GetDirectories().OrderBy(p => p.CreationTime).ToArray();

            foreach (DirectoryInfo directoryTheme in directoriesTheme)
            {
                if (directoryTheme.Name != "JSON" && directoryTheme.Name != "UtilPrefabs")
                {
                    themeStr += directoryTheme.Name + ";";
                }
            }

            File.WriteAllText(Application.dataPath + RESOURCES_FOLDER + "/" + THEME_FILENAME, themeStr);
        }
    }

    /// <summary>
    /// Retrieves the contents of a text file placed in the resources folder from a path. 
    /// Assume that each element is separated by a comma.
    /// </summary>
    /// <param name="path">File path from Resources folder</param>
    /// <returns>Return the file content in a string array</returns>
    public static string[] GetTextContentFromResources(string path)
    {
        string text = Resources.Load(path).ToString();
        text = text.Remove(text.Length - 1);

        return text.Split(';');
    }

    /// <summary>
    /// Returns the name of the furniture available according to the style of the apartment and the type of furniture (cabinet, sofa, bed, ...).
    /// If the furniture type exists in the current apartment style folder it returns the contents of the furniture type folder,
    /// Otherwise it returns the contents of the folder of the furniture type in question of the default apartment style. 
    /// This information is contained in files in .txt format.
    /// </summary>
    /// <param name="furnitureType">Current furniture type (cabinet, sofa, bed, ...)</param>
    /// <param name="furnitureStyle">Furniture style in string</param>
    /// <param name="fileExtension">Files extension to consider</param>
    /// <returns>String list of file paths for a given furniture type and apartment style</returns>
    public static List<string> GetFurnitureNamesFromFurnitureType(string furnitureType, string furnitureStyle, string fileExtension = ".prefab")
    {
        List<string> filePaths = new List<string>();
        string[] availableFurnitureType = GetTextContentFromResources(furnitureStyle + "/furnitureType");

        if(availableFurnitureType.Contains(furnitureType))
        {
            foreach(string element in GetTextContentFromResources(furnitureStyle + "/" + furnitureType + "/prefab"))
            {
                filePaths.Add(furnitureStyle + "/" + furnitureType + "/" + element);
            }
        }
        else
        {
            foreach(string element in GetTextContentFromResources(DEFAULT_FURNITURE_STYLE_NAME + "/" + furnitureType + "/prefab"))
            {
                filePaths.Add(DEFAULT_FURNITURE_STYLE_NAME + "/" + furnitureType + "/" + element);
            }
        }
        return filePaths;
    }
}
