using System.IO;
using UnityEditor;
using UnityEngine;


public class OpenLevelScript : MonoBehaviour
{

    private static string rootFolder = "E:\\work\\d2_assets";
    private static string presetFolder = "hd\\env\\preset\\act1\\barracks";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
