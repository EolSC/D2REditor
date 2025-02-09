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

        protected virtual void LoadResource()
        {

        }

        private void TryLoadResource(ResourceCache cache)
        {
            var res_path = path.ToLower();
            object cached = null;
            if (cache.Get(res_path, ref cached))
            {
                resource = cached;
                return;
            }
            LoadResource();
            cache.AddResource(res_path, resource);
        }

        public object GetResource(ResourceCache cache)
        {
            if (resource == null)
            {
                TryLoadResource(cache);
            }
            return resource;
        }
    }


}