/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_Dropdown ddTheme;
    public Toggle toggleDarkMode;
    public Text textFileSelector;
    public GameObject panelMenu;

    void Start()
    {
        // Clean dropdown and toggle menu
        ddTheme.options.Clear();
        toggleDarkMode.isOn = false;
        panelMenu.SetActive(true);

        FolderUtils.InitThemeFolder();

        buildMenuTheme();
    }

    private void buildMenuTheme()
    {
        foreach (string theme in FolderUtils.GetTextContentFromResources("theme"))
        {
            ddTheme.options.Add(new OptionData(theme));
        }
    }



    // Get options selected, config the data in MainConfig and load the scene
    public void GeneratePlan()
    {
        if(textFileSelector.text != "")
        {
            panelMenu.SetActive(false);

            List<OptionData> themeOptions = ddTheme.options;

            MainConfig.Theme = themeOptions[ddTheme.value].text;

            if (toggleDarkMode.isOn)
            {
                MainConfig.Mode = "nightMat";
            }
            else
            {
                MainConfig.Mode = "dayMat";
            }

            SceneManager.LoadScene("SampleScene");
        }
    }

    private Rect windowRect = new Rect(20, 20, 120, 50);

    public void AboutMenu()
    {
        windowRect = GUI.Window(0, windowRect, WindowFunction, "My Window");
    }

    void WindowFunction(int windowID)
    {
        // Draw any Controls inside the window here
    }

    public void QuitMenu()
    {
        Application.Quit();
    }
}
