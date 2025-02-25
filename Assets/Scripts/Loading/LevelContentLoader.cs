using Diablo2Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;



/*
 * Performs all the work related to reading source ds1/json.
 * Result of loading is LevelComponent object
 */
public class LevelContentLoader 
{
    private static string DEFAULT_LEVEL_NAME = "level";
    private LevelLoadingStrategy strategy;
    private ObjectsLoader objectsLoader;

    public LevelContentLoader(LevelLoadingStrategy strategy)
    {
        this.strategy = strategy;
        objectsLoader = new ObjectsLoader(strategy);
    }
    /*
     * Load level content. This function fully constructs LevelPreset and Ds1Preset within Scene
     */
    public bool LoadLevel(LevelLoadingContext context)
    {
        GameObject root = new GameObject();
        var component = root.AddComponent<LevelComponent>();
        bool result = true;
        if (context.clearCache)
        {
            strategy.cache.Clear();
            strategy.dt1Cache.Clear();
        }
        component.Load(context, strategy);
        D2RHierarchyLoader drawer = new D2RHierarchyLoader(strategy);
        if (context.instantiate)
        {
            drawer.InstantiateContent(root, component, objectsLoader, context.displayProgress);
        }
        else
        {
            drawer.FlipRootObject(component);
        }
        if (context.test)
        {
            result = component.Test(context.ds1Content, context.jsonContent);
        }
        if (context.displayProgress)
        {
            Debug.Log("Level loaded: " + context.name);
            EditorUtility.ClearProgressBar();
        }

        return result;
    }

    /*
     * Save current level on disk
     */
    public void SaveLevel(string fileName)
    {
        var level = FindLevel();
        if (level != null)
        {
            level.Save(fileName);
        }
    }

    public static string GetLevelName()
    {
        var level = FindLevel();
        if (level != null)
        {
            return level.GetName() + PathMapper.DS1_EXT;
        }
        return DEFAULT_LEVEL_NAME + PathMapper.DS1_EXT;
    }

    private static LevelComponent FindLevel()
    {
        // if level is loaded(it can be only one level)
        return UnityEngine.Object.FindAnyObjectByType<LevelComponent>() as LevelComponent;
    }

    /*
     * Clean up all the objects before loading
     */
    public static void ClearScene()
    {
       LevelComponent[] objects = UnityEngine.Object.FindObjectsByType<LevelComponent>(FindObjectsSortMode.InstanceID);
        foreach (var obj in objects)
        {
            GameObject gameObject = obj.gameObject;
            UnityEngine.Object.DestroyImmediate(gameObject, true);
        }
    }

    /*
     * Test function - compares preset state with provided reference data.
     */
    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
    {
        var level = FindLevel();
        if (level != null)
        {
            return level.Test(ds1Content, jsonContent);
        }
        return false;
    }

    public void OpenTestLevel(LevelLoadingContext context)
    {
        ClearScene();
        var path = strategy.settings.developer.testLevel;
        OpenLevel(path, context);
    }
    public void OpenLevel(string path, LevelLoadingContext context)
    {
        var paths = strategy.settings.paths;
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */

        string absolute_path = paths.GetAbsolutePath(path);
        string fileName = Path.GetFileNameWithoutExtension(absolute_path);
        string local_path = paths.GetLocalPath(absolute_path);
        string pathToJson = paths.GetPresetForLevel(local_path);
        byte[] dsContent = { };
        string jsonContent = "";

        bool levelExists = File.Exists(absolute_path);
        bool jsonExists = File.Exists(pathToJson);
        if (levelExists)
        {

            dsContent = File.ReadAllBytes(absolute_path);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Level not found " + absolute_path);
        }

        if (jsonExists)
        {

            jsonContent = File.ReadAllText(pathToJson);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Preset for level " + absolute_path + " is not found in path " + pathToJson);
        }

        if (levelExists && jsonExists)
        {
            context.ds1Content = dsContent;
            context.jsonContent = jsonContent;
            context.name = fileName;
            // Load preset
            bool result = LoadLevel(context);
            if (!result)
            {
                throw new Exception("Loading failed: " + path);
            }
        }
    }

}
