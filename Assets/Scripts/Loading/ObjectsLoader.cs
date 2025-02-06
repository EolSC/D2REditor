using Diablo2Editor;
using SimpleJSON;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectsLoader
{
    private ObjectPreset preset;
    private ObjectsMap map = null;
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

    public string FindObjectPreset(int act, long type, long id)
    {
        if (map == null)
        {
            map = new ObjectsMap();
        }
        string presetName = map.FindObjectPresetName(act, type, id);
        if (presetName.Length > 0) {
            // TODO: find preset in monsters and npc folders for monsters
            // and in objects for objects
            return presetName;
        }
        return "";
    }
}
