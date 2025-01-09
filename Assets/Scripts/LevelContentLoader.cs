using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class LevelContentLoader 
{

    private LevelPreset preset = null;
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
        preset = JsonUtility.FromJson<LevelPreset>(jsonContent);
        preset.CreateComponents(jsonContent);
    }



    private void LoadDS1Content(byte[] ds1Content)
    {
        old_content = ds1Content;
    }

    public bool TestLevelLoading(byte[] ds1Content, string jsonContent)
    {
        string resultJson = JsonUtility.ToJson(preset);
        byte[] resultDS1 = old_content;
        bool oldContentEqual = resultDS1.Equals(ds1Content);
        bool jsonEqual = resultJson.Equals(jsonContent);
        return oldContentEqual && jsonEqual;
    }
}
