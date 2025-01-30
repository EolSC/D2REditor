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
        DS1LevelInfo info = Settings().developer.testLevel;
        OpenLevel(info, true, true);
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


    private static void OpenLevel(DS1LevelInfo info, bool test_serialization, bool instantiate)
    {
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */
        string absolute_path = Settings().paths.GetAbsolutePath(info.path);
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

        loader.LoadLevel(info, fileName, dsContent, jsonContent);
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
