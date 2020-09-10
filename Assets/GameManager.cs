/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject firstPerson;
    public GameObject apartmentGO;

    public Material nightMat;
    public Material dayMat;

    public Light pointLight;
    public Light directionalLight;

    public GameObject canvas;
    public GameObject bigMap;
    public GameObject miniMap;
    public GameObject MiniMapCamera;

    public RenderTexture rtMinimapBlueprint;
    public RenderTexture rtMinimapAppart;

    private RenderTexture rtBigMapBluePrint;
    private static bool blueprintMode = false;
    private new Camera camera;
    private RawImage rawImage;

    void Start()
    {
        var rect = canvas.GetComponent<RectTransform>();
        var big = bigMap.GetComponent<RectTransform>();
        var bigTexture = bigMap.GetComponent<RawImage>();

        rtBigMapBluePrint = new RenderTexture(Convert.ToInt32(rect.sizeDelta.x), Convert.ToInt32(rect.sizeDelta.y), 16, RenderTextureFormat.ARGB32);
        rtBigMapBluePrint.Create();

        bigMap.transform.position = canvas.transform.position;
        big.sizeDelta = rect.sizeDelta;
        bigTexture.texture = rtBigMapBluePrint;

        camera = MiniMapCamera.GetComponent<Camera>();
        rawImage = miniMap.GetComponent<RawImage>();

        FolderUtils.InitFurnitureFolders();

        Plan plan = JSONParser.ParsePlan(MainConfig.Plan);

        plan.Extrude();

        SetCameraStartPosition(plan);

        FurnitureRules fr = JSONParser.ParseFurnitureRules();
        FurniturePlacement fp = new FurniturePlacement(plan, fr, apartmentGO);
        fp.Process();

        plan.DestroyWindowCollider();
        plan.DestroyDoorCollider();

        if(MainConfig.Mode == "dayMat") // dayMat or nightMat
        {
            RenderSettings.skybox = dayMat; 
        }
        else
        {
            RenderSettings.skybox = nightMat;
        }

        directionalLight.intensity = 1f; // 1.f
    }

    /// <summary>
    /// Set the position of the camera from the start point specify in the plan
    /// </summary>
    /// <param name="plan">Apartment plan</param>
    private void SetCameraStartPosition(Plan plan)
    {
        firstPerson.transform.position = new Vector3(plan.entryPoint[0], 1.0f, plan.entryPoint[1]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(blueprintMode)
            {
                blueprintMode = !blueprintMode;
                UnsetBlueprintMode();
            }
            else
            {
                blueprintMode = !blueprintMode;
                SetBlueprintMode();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Switch the view between minimap and FPV (blueprint as big and FPV as minimap)
    /// </summary>
    private void SetBlueprintMode()
    {
        bigMap.SetActive(true);

        camera.targetTexture = rtBigMapBluePrint;
        rawImage.texture = rtMinimapAppart;

        SwitchLayerCollision(true);
    }

    /// <summary>
    /// Switch the view between minimap and FPV (FPV as big and blueprint as minimap)
    /// </summary>
    private void UnsetBlueprintMode()
    {
        bigMap.SetActive(false);

        camera.targetTexture = rtMinimapBlueprint;
        rawImage.texture = rtMinimapBlueprint;

        SwitchLayerCollision(false);
    }

    private void SwitchLayerCollision(bool b)
    {
        Physics.IgnoreLayerCollision(12, 16, b);
        Physics.IgnoreLayerCollision(0, 16, b);
        Physics.IgnoreLayerCollision(8, 16, b);
    }
}
