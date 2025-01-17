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

        public virtual void LoadResource()
        {

        }

        public object GetResource()
        {
            return resource;
        }
    }


}