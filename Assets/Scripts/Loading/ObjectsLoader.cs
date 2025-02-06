using Diablo2Editor;
using SimpleJSON;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectsLoader
{
    private ObjectPreset preset;
    public void Load(string path, bool instantiate)
    {
        string full_path = EditorMain.Settings().paths.GetAbsolutePath(path);
        if (File.Exists(full_path))
        {
            string name = Path.GetFileNameWithoutExtension(full_path);
            GameObject gameObject = new GameObject();
            gameObject.name = name;

            string jsonContent = File.ReadAllText(full_path);
            JSONNode jsonNode = JSON.Parse(jsonContent);
            preset = new ObjectPreset(gameObject);
            preset.Deserialize(jsonNode.AsObject);
            if (instantiate)
            {
                EditorUtility.DisplayProgressBar("Loading level", "Loading resources...", 0.0f);
                preset.LoadResources();
                preset.Instantiate();
                EditorUtility.ClearProgressBar();
            }
        }
    }

    public string FindObjectPreset(long type, long id)
    {
        string contentFile = "";
        PathMapper pathMapper = EditorMain.Settings().paths;
        switch (type)
        {
            
            case 1:
                {
                    contentFile = pathMapper.GetMonstersPath();

                }; break;
            case 2:
                {
                    contentFile = pathMapper.GetObjectsPath();
                }; break;
            default:
                break;
        }
        if (contentFile.Length > 0)
        {
            return FindPreset(id, contentFile);
        }
        return "";
    }

    private string FindPreset(long id, string fileName)
    {
        if (File.Exists(fileName))
        {
            string jsonContent = File.ReadAllText(fileName);

            JSONNode jsonNode = JSON.Parse(jsonContent);

            return "";
        }
        return "";
    }

}
