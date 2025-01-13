using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class LevelPresetDependency : ISerializable
    {
        public string path;

        public override void Deserialize(JSONObject json)
        {
            path = json["path"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["path"] = path;
            return result;
        }
    }
}