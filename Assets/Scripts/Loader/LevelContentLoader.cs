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
        LoadJsonPreset(levelName, jsonContent);
        LoadDS1Content(pathToLevel, ds1Content);
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
        DS1Loader loader = new DS1Loader();
        ds1Level = loader.ReadDS1(ds1Content);
        LevelTypesLoader levelTypes = new LevelTypesLoader();
        var tileTables = levelTypes.FindTilesForLevel(pathToLevel);
        if (tileTables.Count > 0)
        {
            ds1Level.InitBlockTable(tileTables);
            GameObject testDS1 = new GameObject();
            ds1Level.Intantiate(testDS1);
        }


    }

    /*
     * Test funcion - compares preset state with provided reference data.
     */
    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
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

    public void Instantiate()
    {
        if (preset != null)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loding resources...", 0.0f);
            preset.LoadResources();


            EditorUtility.DisplayProgressBar("Loading level", "Instantiating objects...", 0.8f);
            preset.Instantiate();
            EditorUtility.ClearProgressBar();
            UpdateCameraSettings(preset.gameObject);
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
