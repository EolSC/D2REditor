using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;


public class OpenLevelScript : MonoBehaviour
{

    public static string rootFolder = "E:\\work\\d2_assets";
    public static string presetFolder = "data\\hd\\env\\preset\\act1\\barracks";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string GetAbsolutePath(string fileName)
    {
        var canonical_path = Path.GetFullPath(Path.Combine(rootFolder, fileName));
        return canonical_path;
    }

    [MenuItem("Diablo Level Editor/Test loading")]
    private static void TestLoading()
    {
        string pathToLevel = GetAbsolutePath("data\\global\\tiles\\act1\\barracks\\barew.ds1");
        string fileName = Path.GetFileNameWithoutExtension(pathToLevel);
        string pathToJson = System.IO.Path.Combine(rootFolder, presetFolder, fileName + ".json");
        byte[] dsContent = { };
        string jsonContent = "";
        if (File.Exists(pathToLevel))
        {

            dsContent = File.ReadAllBytes(pathToLevel);
        }
        if (File.Exists(pathToJson))
        {

            jsonContent = File.ReadAllText(pathToJson);
        }

        LevelContentLoader loader = new LevelContentLoader();
        loader.LoadLevel(fileName, dsContent, jsonContent);
        bool loadingResult = loader.TestLevelLoading(dsContent, jsonContent);
        if (loadingResult)
        {
            Debug.Log("Test loading: success");
        }
        else
        {
            Debug.Log("Test loading: failure");
        }

    }

    [MenuItem("Diablo Level Editor/Open level")]
    private static void OpenLevel()
    {
        string pathToLevel = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        string fileName = Path.GetFileNameWithoutExtension(pathToLevel);
        string pathToJson = System.IO.Path.Combine(rootFolder, presetFolder, fileName + ".json");
        byte[] dsContent = {};
        string jsonContent = "";
        if (File.Exists(pathToLevel))
        {
            
            dsContent = File.ReadAllBytes(pathToLevel);
        }
        if (File.Exists(pathToJson))
        {

            jsonContent = File.ReadAllText(pathToJson);
        }

        LevelContentLoader loader = new LevelContentLoader();
        loader.LoadLevel(fileName, dsContent, jsonContent);

    }



}
