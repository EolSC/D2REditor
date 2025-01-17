using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
     * Container to hold all level dependencies
     * Holds actual resources data and passes it to compoonents for instancing
     */

    public class LevelPresetDependencies : ISerializable
    {
        public Dictionary<string, List<LevelPresetDependency>> dependencies = new Dictionary<string, List<LevelPresetDependency>>();


        public Object GetResource(string resourcePath, DependencyType type)
        {
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
                case DependencyType.Texture:
                    {
                        return ISerializable.DeserializeList<DependencyTexture, LevelPresetDependency>(obj, GetJsonAttributeName(type));
                    };
                case DependencyType.Model:
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
                JSONObject source = obj[name] as JSONObject;
                if (source)
                {
                    dependencies[name] = LoadDependencyList(source, depType);
                    foreach (var dep in dependencies[name])
                    {
                        dep.dependencies = this;
                    }
                }
            }
        }

        public void LoadResources()
        {
            foreach (var value in dependencies.Values)
            {
                foreach(var item in value)
                {
                    item.LoadResource();
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
