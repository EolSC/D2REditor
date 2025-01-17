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
    // Random seed for level preset
    private int seed = 0;
    // Old DS1 data(not handled yet)
    private byte[] oldContent = { };
    
    /*
     * Load level content. This function fully constructs LevelPreset and Ds1Preset within Scene
     */

    public void LoadLevel(string levelName, byte[] ds1Content, string jsonContent)
    {
        LoadJsonPreset(levelName, jsonContent);
        LoadDS1Content(ds1Content);
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
    private void LoadDS1Content(byte[] ds1Content)
    {
        oldContent = ds1Content;
    }

    /*
     * Test funcion - compares preset state with provided reference data.
     */
    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
    {
        JSONNode resultJson = preset.Serialize();
        JSONNode sourceJson = JSON.Parse(jsonContent);

        byte[] resultDS1 = oldContent;
        bool oldContentEqual = resultDS1.Equals(ds1Content);
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

        }
    }




}
