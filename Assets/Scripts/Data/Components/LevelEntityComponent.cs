using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{

    public class LevelEntityComponent : ISerializable
    {
        public string type;
        public string name;

        public override void Deserialize(JSONObject json)
        {
            type = json["type"];
            name = json["name"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = type;
            result["name"] = name;
            return result;
        }

    }
}