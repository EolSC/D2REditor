using Diablo2Editor;
using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;


public class OpenLevelScript : MonoBehaviour
{

    private PathMapper pathMapper;

    private static void OpenLevel(string absolute_path, bool test_serialization)
    {
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

        LevelContentLoader loader = new LevelContentLoader();
        loader.LoadLevel(fileName, dsContent, jsonContent);
        if (test_serialization)
        {
            loader.TestLevelLoading(dsContent, jsonContent);
        }
    }

    [MenuItem("Diablo Level Editor/Test loading")]
    private static void TestLoading()
    {
        PathMapper.InitBaseFolder();
        string pathToLevel = PathMapper.GetTestLevel();
        string absolute_path = PathMapper.GetAbsolutePath(pathToLevel);
        OpenLevel(absolute_path, true);
    }

    [MenuItem("Diablo Level Editor/Open level")]
    private static void OpenLevel()
    {
        PathMapper.InitBaseFolder();
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        OpenLevel(absolute_path, false);
    }



}
