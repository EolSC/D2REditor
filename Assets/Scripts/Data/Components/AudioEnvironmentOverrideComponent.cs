using UnityEngine;
using SimpleJSON;


namespace Diablo2Editor
{
    public class AudioEnvironmentOverrideComponent : LevelEntityComponent
    {
        string overrideEnvironment;
        Vector3 min = new Vector3();
        Vector3 max = new Vector3();

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            overrideEnvironment = json["overrideEnvironment"];

            var minJson = json["min"].AsObject;
            min.x = ISerializable.DeserializeFloat(minJson["x"]);
            min.y = ISerializable.DeserializeFloat(minJson["y"]);
            min.z = ISerializable.DeserializeFloat(minJson["z"]);

            var maxJson = json["max"].AsObject;
            max.x = ISerializable.DeserializeFloat(maxJson["x"]);
            max.y = ISerializable.DeserializeFloat(maxJson["y"]);
            max.z = ISerializable.DeserializeFloat(maxJson["z"]);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["overrideEnvironment"] = overrideEnvironment;

            var minJson = new JSONObject();
            minJson["x"] = ISerializable.SerializeFloat(min.x);
            minJson["y"] = ISerializable.SerializeFloat(min.y);
            minJson["z"] = ISerializable.SerializeFloat(min.z);
            result["min"] = minJson;

            var maxJson = new JSONObject();
            maxJson["x"] = ISerializable.SerializeFloat(max.x);
            maxJson["y"] = ISerializable.SerializeFloat(max.y);
            maxJson["z"] = ISerializable.SerializeFloat(max.z);
            result["max"] = maxJson;

            return result;
        }
    }
}