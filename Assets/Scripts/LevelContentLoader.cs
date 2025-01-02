using UnityEditor;
using UnityEngine;


public class LevelContentLoader 
{

    private LevelPreset preset = null;
    private byte[] old_content = { }; 

    public void LoadLevel(string levelName, byte[] ds1Content, string jsonContent)
    {
        preset = JsonUtility.FromJson<LevelPreset>(jsonContent);
        old_content = ds1Content;
        Debug.Log("Level loaded " + levelName);
        Debug.Log("Preset name is " + preset.biomeFilename);
        Debug.Log("Ds1 content length " + old_content.Length);


    }
}
