using UnityEngine;
using SimpleJSON;
using System;
using System.IO;
using System.Linq;

namespace Diablo2Editor
{
    /*
     * Link to any file used in level
     */
    public class LevelPresetDependency : ISerializable
    {
        public string path;
        public LevelPresetDependencies dependencies;
        protected object resource;

        public void Deserialize(JSONObject json)
        {
            path = json["path"];
        }

        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["path"] = path;
            return result;
        }

        protected virtual void LoadResource(LevelLoadingStrategy strategy)
        {

        }

        private void TryLoadResource(LevelLoadingStrategy strategy)
        {
            var res_path = path.ToLower();
            var cache = strategy.cache;
            object cached = null;
            if (cache.Get(res_path, ref cached))
            {
                resource = cached;
                return;
            }
            LoadResource(strategy);
            cache.AddResource(res_path, resource);
        }

        public object GetResource(LevelLoadingStrategy strategy)
        {
            if (resource == null)
            {
                TryLoadResource(strategy);
            }
            return resource;
        }
    }


}