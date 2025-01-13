using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class TransformDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 scale;
        public bool inheritOnlyPosition = true;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject position = obj["position"].AsObject;
            this.position.x = DeserializeFloat(position["x"]);
            this.position.y = DeserializeFloat(position["y"]);
            this.position.z = DeserializeFloat(position["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = DeserializeFloat(orientation["x"]);
            this.orientation.y = DeserializeFloat(orientation["y"]);
            this.orientation.z = DeserializeFloat(orientation["z"]);
            this.orientation.w = DeserializeFloat(orientation["w"]);

            JSONObject scale = obj["scale"].AsObject;
            this.scale.x = DeserializeFloat(scale["x"]);
            this.scale.y = DeserializeFloat(scale["y"]);
            this.scale.z = DeserializeFloat(scale["z"]);

            this.inheritOnlyPosition = obj["inheritOnlyPosition"].AsBool;
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject pos_object = new JSONObject();
            pos_object["x"] = SerializeFloat(this.position.x);
            pos_object["y"] = SerializeFloat(this.position.y);
            pos_object["z"] = SerializeFloat(this.position.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = SerializeFloat(this.orientation.x);
            orientation_object["y"] = SerializeFloat(this.orientation.y);
            orientation_object["z"] = SerializeFloat(this.orientation.z);
            orientation_object["w"] = SerializeFloat(this.orientation.w);

            JSONObject scale_object = new JSONObject();
            scale_object["x"] = SerializeFloat(this.scale.x);
            scale_object["y"] = SerializeFloat(this.scale.y);
            scale_object["z"] = SerializeFloat(this.scale.z);

            result["position"] = pos_object;
            result["orientation"] = orientation_object;
            result["scale"] = scale_object;
            result["inheritOnlyPosition"] = this.inheritOnlyPosition;

            return result;
        }
    }
}