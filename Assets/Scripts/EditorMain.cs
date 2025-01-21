using Diablo2Editor;
using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;


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

    [MenuItem("Diablo Level Editor/Test loading")]
    private static void TestLoading()
    {
        // Use path from settings without opening Browse dialog
        string pathToLevel = Settings().developer.testLevel;
        string absolute_path = Settings().paths.GetAbsolutePath(pathToLevel);
        OpenLevel(absolute_path, true, true);
    }

    [MenuItem("Diablo Level Editor/Open level...")]
    private static void OpenLevel()
    {
        // Open file dialog
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        // Load level with no tests
        OpenLevel(absolute_path, true, false);
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


    private static void OpenLevel(string absolute_path, bool test_serialization, bool instantiate)
    {
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */
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
        LevelContentLoader loader = new LevelContentLoader();

        EditorUtility.DisplayProgressBar("Loading level", "Reading json...", 0.0f);
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
