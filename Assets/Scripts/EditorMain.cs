using Diablo2Editor;
using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


/*
 * Static data. Settings and global classes are here
 */
namespace Diablo2Editor
{

}
/* 
 * Entry-point class for this project. Provides Menu items, defines basic functionality of editor
 */

public class EditorMain : MonoBehaviour
{
    public static Diablo2Editor.EditorSettings settings = new Diablo2Editor.EditorSettings();

    private static LevelContentLoader loader = new LevelContentLoader();

    [MenuItem("Diablo Level Editor/Open level...")]
    private static void OpenLevel()
    {
        // Open file dialog
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        // Load level with no tests
        OpenLevel(absolute_path);
    }

    [MenuItem("Diablo Level Editor/Save level")]
    private static void SaveLevel()
    {
        var path = EditorUtility.SaveFilePanel(
            "Save level as ds1/json pair",
            "",
            "level.ds1",
            "ds1");

        if (path.Length != 0)
        {
            loader.SaveLevel(path);
        }
    }

    [MenuItem("Diablo Level Editor/Test loading")]
    private static void TestLoading()
    {
        // Use path from settings without opening Browse dialog
        string path = Settings().developer.testLevel;
        OpenLevel(path, true, true);
    }


    [MenuItem("Diablo Level Editor/Settings/Reload")]
    private static void ReloadSettings()
    {
        Settings().Reload();
    }

    public static Diablo2Editor.EditorSettings Settings()
    {
        return settings;
    }


    private static void OpenLevel(string path, bool instantiate = true, bool test_serialization = false)
    {
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */
        string absolute_path = Settings().paths.GetAbsolutePath(path);
        string fileName = Path.GetFileNameWithoutExtension(absolute_path);
        string local_path = Settings().paths.GetLocalPath(absolute_path);
        string pathToJson = Settings().paths.GetPresetForLevel(local_path);
        byte[] dsContent = { };
        string jsonContent = "";
        if (File.Exists(absolute_path))
        {

            dsContent = File.ReadAllBytes(absolute_path);
        }
        if (File.Exists(pathToJson))
        {

            jsonContent = File.ReadAllText(pathToJson);
        }
        // Load preset
        loader.LoadLevel(fileName, dsContent, jsonContent);
        //Perform some tests if they are needed
        if (test_serialization)
        {
            loader.TestLevelLoading(dsContent, jsonContent);
        }
        if (instantiate)
        { // instantiate it
            loader.Instantiate();
        }
    }



}
