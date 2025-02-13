using Diablo2Editor;
using SimpleJSON;
using System.Collections.Generic;
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
    /*
     * Load level content. This function fully constructs LevelPreset and Ds1Preset within Scene
     */
    public void LoadLevel(string levelName, byte[] ds1Content, string jsonContent, bool instantiate, bool displayProgress, bool loadJson)
    {
        GameObject root = new GameObject();
        var component = root.AddComponent<LevelComponent>();
        component.Load(levelName, ds1Content, jsonContent, instantiate, displayProgress, loadJson); 
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
        return Object.FindAnyObjectByType<LevelComponent>() as LevelComponent;
    }

    /*
     * Clean up all the objects before loading
     */
    public static void ClearScene()
    {
       LevelComponent[] objects = Object.FindObjectsByType<LevelComponent>(FindObjectsSortMode.InstanceID);
        foreach (var obj in objects)
        {
            GameObject gameObject = obj.gameObject;
            Object.DestroyImmediate(gameObject, true);
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
}
