using NUnit.Framework;
using SimpleJSON;
using UnityEditor;
using UnityEngine;


public class LevelContentLoader 
{

    private Diablo2Editor.LevelPreset preset = null;
    private byte[] old_content = { }; 

    public void LoadLevel(string levelName, byte[] ds1Content, string jsonContent)
    {
        LoadJsonPreset(jsonContent);
        LoadDS1Content(ds1Content);

        Debug.Log("Level loaded " + levelName);
        Debug.Log("Preset name is " + preset.biomeFilename);
        Debug.Log("Ds1 content length " + old_content.Length);


    }

    private void LoadJsonPreset(string jsonContent)
    {
        JSONNode root = JSON.Parse(jsonContent);
        preset = new Diablo2Editor.LevelPreset();
        preset.Deserialize(root.AsObject);
    }



    private void LoadDS1Content(byte[] ds1Content)
    {
        old_content = ds1Content;
    }

    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
    {
        JSONNode resultJson = preset.Serialize();
        JSONNode sourceJson = JSON.Parse(jsonContent);

        byte[] resultDS1 = old_content;
        bool oldContentEqual = resultDS1.Equals(ds1Content);
        bool jsonEqual = CompareJson(sourceJson, resultJson);

        return oldContentEqual && jsonEqual;
    }


    private bool CompareJson(JSONNode first, JSONNode second)
    {
        if (first.IsObject && second.IsObject) { 
            return CompareJsonObjects(first, second);
        }
        if (first.IsNull && second.IsNull)
        {
            return true;
        }
        if (first.IsArray && second.IsArray)
        {
            return CompareJsonArray(first, second);
        }
        if (first.IsBoolean && second.IsBoolean)
        {
            return first.AsBool == second.AsBool;
        }
        if (first.IsNumber && second.IsNumber)
        {
            return CompareJsonNumbers(first, second);
        }
        if (first.IsString && second.IsString)
        {
            return first.ToString() == second.ToString();
        }
        return true;
    }

    private bool CompareJsonArray(JSONNode first, JSONNode second)
    {
        return true;
    }

    private bool CompareJsonObjects(JSONNode first, JSONNode second)
    {
        return true;
    }

    private bool CompareJsonNumbers(JSONNode first, JSONNode second)
    {
        return true;
    }
}
