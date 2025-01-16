using Diablo2Editor;
using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;


/* 
 * Entry-point class for this project. Provides Menu items, defines basic functionality of editor
 */
public class OpenLevelScript : MonoBehaviour
{

    private PathMapper pathMapper;

    private static void OpenLevel(string absolute_path, bool test_serialization)
    {
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */
        string fileName = Path.GetFileNameWithoutExtension(absolute_path);
        string local_path = PathMapper.GetLocalPath(absolute_path);
        string pathToJson = PathMapper.GetPresetForLevel(local_path);
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
        loader.LoadLevel(fileName, dsContent, jsonContent);
        //Perform some tests if they are needed
        if (test_serialization)
        {
            loader.TestLevelLoading(dsContent, jsonContent);
        }
    }

    [MenuItem("Diablo Level Editor/Test loading")]
    private static void TestLoading()
    {
        PathMapper.InitBaseFolder();
        // Use path from settings without opening Browse dialog
        string pathToLevel = PathMapper.GetTestLevel();
        string absolute_path = PathMapper.GetAbsolutePath(pathToLevel);
        OpenLevel(absolute_path, true);
    }

    [MenuItem("Diablo Level Editor/Open level")]
    private static void OpenLevel()
    {
        PathMapper.InitBaseFolder();
        // Open file dialog
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        // Load level with no tests
        OpenLevel(absolute_path, false);
    }



}
