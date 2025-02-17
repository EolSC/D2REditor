using SimpleJSON;
using UnityEngine;
using UnityEngine.Rendering;
namespace Diablo2Editor
{
    public class LevelHeightOffsetComponent : LevelEntityComponent
    {
        public float down = 0.0f;
        public float left = 0.0f;
        public float right = 0.0f;
        public float up = 0.0f;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            down = ISerializable.DeserializeFloat(json["down"]);
            left = ISerializable.DeserializeFloat(json["left"]);
            right = ISerializable.DeserializeFloat(json["right"]);
            up = ISerializable.DeserializeFloat(json["up"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["down"] = ISerializable.SerializeFloat(down);
            result["left"] = ISerializable.SerializeFloat(left);
            result["right"] = ISerializable.SerializeFloat(right);
            result["up"] = ISerializable.SerializeFloat(up);

            return result;
        }
    }
}
