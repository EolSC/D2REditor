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
            min = ISerializable.DeserializeVector(json["min"].AsObject);
            max = ISerializable.DeserializeVector(json["max"].AsObject);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["overrideEnvironment"] = overrideEnvironment;

            result["min"] = ISerializable.SerializeVector(min);
            result["max"] = ISerializable.SerializeVector(max);

            return result;
        }
    }
}