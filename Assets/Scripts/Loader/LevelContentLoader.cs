using Diablo2Editor;
using SimpleJSON;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * Performs all the work related to reading source ds1/json.
 * Result of loading is LevelPreset object
 */
public class LevelContentLoader 
{
    // Loaded LevelPreset(D2R json data)
    private Diablo2Editor.LevelPreset preset = null;
    // Loaded Level(D2 level data)
    private Diablo2Editor.DS1Level ds1Level = null;
    // Random seed for level preset
    private int seed = 0;
    
    /*
     * Load level content. This function fully constructs LevelPreset and Ds1Preset within Scene
     */

    public void LoadLevel(string pathToLevel, string levelName, byte[] ds1Content, string jsonContent)
    {
        EditorUtility.DisplayProgressBar("Loading level", "Reading json...", 0.0f);
        LoadJsonPreset(levelName, jsonContent);
        EditorUtility.DisplayProgressBar("Loading level", "Loading DS1...", 0.0f);
        LoadDS1Content(pathToLevel, ds1Content);
        EditorUtility.ClearProgressBar();
        Debug.Log("Level loaded: " + levelName);
    }

    // Json-specific logic
    private void LoadJsonPreset(string levelName, string jsonContent)
    {
        GameObject rootGameObject = new GameObject();
        rootGameObject.name = levelName;
        JSONNode jsonNode = JSON.Parse(jsonContent);
        preset = new Diablo2Editor.LevelPreset(rootGameObject, jsonNode.AsObject, seed);
      }


    // Ds1-specific logic
    private void LoadDS1Content(string pathToLevel, byte[] ds1Content)
    {
        // Reload palettes
        D2Palette palletes = new D2Palette();
        palletes.LoadPalleteFiles();

        DS1Loader loader = new DS1Loader();
        ds1Level = loader.ReadDS1(ds1Content);
        LevelTypesLoader levelTypes = new LevelTypesLoader();
        var tileTables = levelTypes.FindTilesForLevel(pathToLevel, palletes);
        if (tileTables.Count > 0)
        {
            ds1Level.InitBlockTable(tileTables);
        }


    }

    /*
     * Test funcion - compares preset state with provided reference data.
     */
    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
    {
        if ((preset != null) && (ds1Level != null))
        {
            JSONNode resultJson = preset.Serialize();
            JSONNode sourceJson = JSON.Parse(jsonContent);

            bool oldContentEqual = false;
            if (ds1Level != null)
            {
                oldContentEqual = ds1Level.test_data.Equals(ds1Content);
            }
            bool jsonEqual = JSONCompare.Compare(sourceJson, resultJson);

            return oldContentEqual && jsonEqual;
        }
        return false;
    }

    public void Instantiate()
    {
        
        if (preset != null)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading resources...", 0.0f);
            preset.LoadResources();


            EditorUtility.DisplayProgressBar("Loading level", "Instantiating objects...", 0.8f);
            preset.Instantiate();
            EditorUtility.ClearProgressBar();
            UpdateCameraSettings(preset.gameObject);
        }
       
        if (ds1Level != null)
        {
            GameObject testDS1 = new GameObject();
            DS1Drawer drawer = new DS1Drawer();
            drawer.DrawTilesToTexture(ds1Level);
            drawer.Intantiate(testDS1);
        }
    }

    private void UpdateCameraSettings(GameObject gameObject)
    {
        Diablo2Editor.EditorSettings.CameraSettings camera = EditorMain.Settings().camera;
        SceneView.CameraSettings settings = new SceneView.CameraSettings();
        settings.nearClip = camera.nearClip;
        settings.farClip = camera.farClip;
        settings.occlusionCulling = camera.occlusionCulling;

        SceneView sceneView = SceneView.lastActiveSceneView;
        sceneView.orthographic = true;
        sceneView.size = camera.zoom;
        sceneView.cameraSettings = settings;

        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; ++i)
                bounds.Encapsulate(renderers[i].bounds);
            var center = bounds.center;
            sceneView.pivot = center;
        }
        sceneView.Repaint();
    }





}
