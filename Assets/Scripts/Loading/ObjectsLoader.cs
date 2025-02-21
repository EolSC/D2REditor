using Diablo2Editor;
using NUnit.Framework;
using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;

public class ObjectsLoader
{
    private ObjectPreset preset;
    private ObjectsMap map = null;
    private LevelLoadingStrategy strategy;

    public ObjectsLoader(LevelLoadingStrategy strategy)
    {
        this.strategy = strategy;
        map = new ObjectsMap(this.strategy.settings.paths);
    }


    public void Load(int act, long type, long id, GameObject gameObject, bool isReloading)
    {
        var actZeroBased = act - 1;
        var path = FindObjectPreset(actZeroBased, type, id);
        if (path.Length > 0)
        {
            Load(path, gameObject, isReloading);
        }
        else
        {
            gameObject.name = "EmptyObject";
        }
    }

    public void Load(string full_path, GameObject gameObject, bool isReloading)
    {
        if (isReloading)
        {
            foreach (Transform child in gameObject.transform)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }
        if (File.Exists(full_path))
        {
            GameObject child = new GameObject();
            child.name = "json";
            child.transform.parent = gameObject.transform;

            string name = Path.GetFileNameWithoutExtension(full_path);
            gameObject.name = name;

            string jsonContent = File.ReadAllText(full_path);
            JSONNode jsonNode = JSON.Parse(jsonContent);
            preset = new ObjectPreset(child, strategy);
            preset.Deserialize(jsonNode.AsObject);

            preset.Instantiate();

            if(isReloading)
            {
                // reset position 
                child.transform.localPosition = Vector3.zero;

            }
        }
    }

    private string FindObjectPreset(int act, long type, long id)
    {
        string presetName = map.FindObjectPresetName(act, type, id);
        if (presetName.Length > 0) {
            return FindPresetOnDisc(type, presetName);
        }
        return "";
    }

    private string FindPresetOnDisc(long type, string presetName)
    {
        List<string> folders = new List<string>();
        var pathMapper = strategy.settings.paths;
        if (type == 1) // npc or enemy
        {
            // add both folders in search
            folders.Add(pathMapper.GetMonsterRoot());
            folders.Add(pathMapper.GetNPCRoot());
        }
        if (type == 2) // object
        {
            // search objects
            folders.Add(pathMapper.GetObjectsRoot());
        }
        return GetFullPresetName(pathMapper, presetName, folders);
    }

    private string GetFullPresetName(PathMapper mapper, string presetName, List<string> folders)
    {

        foreach (var folder in folders)
        {
            var path = Path.Combine(folder, presetName);
            var abs_path = mapper.GetAbsolutePath(path);
            if (File.Exists(abs_path))
            {
                return abs_path;
            }
        }
        return "";
    }
}
