using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
     * Link to any file used in level
     */
    public class LevelPresetDependency : ISerializable
    {
        public string path;

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
    }
}