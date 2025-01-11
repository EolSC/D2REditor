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
        string resultJson = preset.Serialize().ToString();
        byte[] resultDS1 = old_content;
        bool oldContentEqual = resultDS1.Equals(ds1Content);
        bool jsonEqual = resultJson.Equals(jsonContent);
        return oldContentEqual && jsonEqual;
    }
}
