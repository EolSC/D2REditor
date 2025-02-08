using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEditor;

namespace Diablo2Editor
{
    /*
     * Container to hold all level dependencies
     * Holds actual resources data and passes it to compoonents for instancing
     */

    public class LevelPresetDependencies : ISerializable
    {
        public Dictionary<string, List<LevelPresetDependency>> dependencies = new Dictionary<string, List<LevelPresetDependency>>();


        public object GetResource(string resourcePath, DependencyType type)
        {
            var res_path = resourcePath.ToLower();
            string name = GetJsonAttributeName(type);
            List<LevelPresetDependency> resources = dependencies[name];
            foreach(var resource in resources) 
            {
                if (res_path == resource.path.ToLower())
                {
                    return resource.GetResource();
                }
            }
            Debug.LogError("Resource of type " + type.ToString() + " is not found " + resourcePath);
            return null;
        }

        private string GetJsonAttributeName(DependencyType type)
        {
            string result = type.ToString().ToLower();
            return result;
        }

        private List<LevelPresetDependency> LoadDependencyList(JSONObject obj, DependencyType type)
        {
            switch (type)
            {
                case DependencyType.Textures:
                    {
                        return ISerializable.DeserializeList<DependencyTexture, LevelPresetDependency>(obj, GetJsonAttributeName(type));
                    };
                case DependencyType.Models:
                    {
                        return ISerializable.DeserializeList<DependencyModel, LevelPresetDependency>(obj, GetJsonAttributeName(type));
                    };
                default:
                    {
                        return ISerializable.DeserializeList<LevelPresetDependency>(obj, GetJsonAttributeName(type));
                    };
            }
        }

        public void Deserialize(JSONObject obj)
        {
            dependencies.Clear();
            foreach (DependencyType depType in System.Enum.GetValues(typeof(DependencyType)))
            {
                string name = GetJsonAttributeName(depType);
                dependencies[name] = LoadDependencyList(obj, depType);
                foreach (var dep in dependencies[name])
                {
                    dep.dependencies = this;
                }
            }
        }

        public void LoadResources(bool displayProgress = true)
        {
            if (dependencies.Count > 0)
            {
                float total = 0;
                foreach (var value in dependencies.Values)
                {
                    total += value.Count;
                }

                float startProgress = 0.0f;
                float endProgress = 0.8f;
                float step = (endProgress - startProgress) / total;
                foreach (var value in dependencies.Values)
                {
                    foreach (var item in value)
                    {
                        if (displayProgress)
                        {
                            EditorUtility.DisplayProgressBar("Loading level", "Loading resources...", startProgress);
                        }
                        item.LoadResource();
                        startProgress += step;
                    }
                }

            }
        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            foreach (var pair in  dependencies)
            {
                string name = pair.Key;
                List<LevelPresetDependency> deps = pair.Value;
                result[name] = ISerializable.SerializeList(deps);
            }
            return result;
        }
    }
}
