using SimpleJSON;
using UnityEngine;

namespace Diablo2Editor
{
    public class ParallaxOffsetComponent : LevelEntityComponent
    {
        public Vector3 gameCameraPosition;
        public float ratio = 0.0f;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            gameCameraPosition = ISerializable.DeserializeVector(json["gameCameraPosition"].AsObject);
            ratio = ISerializable.DeserializeFloat(json["ratio"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["gameCameraPosition"] = ISerializable.SerializeVector(gameCameraPosition);
            result["ratio"] = ISerializable.SerializeFloat(ratio);

            return result;
        }
    }
}
