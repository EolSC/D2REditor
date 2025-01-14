using NUnit.Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        GameObject rootGameObject = new GameObject();
        JSONNode jsonNode = JSON.Parse(jsonContent);
        preset = new Diablo2Editor.LevelPreset(jsonNode.AsObject, rootGameObject);
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
            return CompareJsonObjects(first.AsObject, second.AsObject);
        }
        if (first.IsArray && second.IsArray)
        {
            return CompareJsonArrays(first.AsArray, second.AsArray);
        }
        // assume null objects equal
        if (first.IsNull && second.IsNull)
        {
            return true;
        }
        
        if (first.IsBoolean && second.IsBoolean)
        {
            return first.AsBool == second.AsBool;
        }
        if (first.IsNumber && second.IsNumber)
        {
            // approximate comprasion for numbers to avoid floating points errors
            return CompareJsonNumbers(first, second);
        }
        if (first.IsString && second.IsString)
        {
            return first.ToString() == second.ToString();
        }

        // types are not equal - therefore they can't be equal
        return false;
    }

    private bool CompareJsonArrays(JSONArray first, JSONArray second)
    {
        int length = first.Count;
        if (length != second.Count)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            JSONNode a = first[i];
            JSONNode b = second[i];
            if (!CompareJson(a, b))
            {
                return false;
            }
        }
        return true;
    }
    private bool CompareJsonObjects(JSONObject first, JSONObject second)
    {
        int length = first.Count;
        if (length != second.Count)
        {
            return false;
        }

        foreach(KeyValuePair<string, JSONNode> pair in first)
        {

            JSONNode a = first[pair.Key];
            JSONNode b = second[pair.Key];
            if (!CompareJson(a, b))
            {
                return false;
            }
        }
        return true;
    }


    private bool CompareJsonNumbers(JSONNode first, JSONNode second)
    {
        return first.AsFloat == second.AsFloat;
    }

}
