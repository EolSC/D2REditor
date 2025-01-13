using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class PhysicsShapeBox : PhysicsShapeType
    {
        public UnityEngine.Vector3 center;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 extents;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject center = obj["center"].AsObject;
            this.center.x = DeserializeFloat(center["x"]);
            this.center.y = DeserializeFloat(center["y"]);
            this.center.z = DeserializeFloat(center["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = DeserializeFloat(orientation["x"]);
            this.orientation.y = DeserializeFloat(orientation["y"]);
            this.orientation.z = DeserializeFloat(orientation["z"]);
            this.orientation.w = DeserializeFloat(orientation["w"]);

            JSONObject extents = obj["extents"].AsObject;
            this.extents.x = DeserializeFloat(extents["x"]);
            this.extents.y = DeserializeFloat(extents["y"]);
            this.extents.z = DeserializeFloat(extents["z"]);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject center_object = new JSONObject();
            center_object["x"] = SerializeFloat(this.center.x);
            center_object["y"] = SerializeFloat(this.center.y);
            center_object["z"] = SerializeFloat(this.center.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = SerializeFloat(this.orientation.x);
            orientation_object["y"] = SerializeFloat(this.orientation.y);
            orientation_object["z"] = SerializeFloat(this.orientation.z);
            orientation_object["w"] = SerializeFloat(this.orientation.w);

            JSONObject extents_object = new JSONObject();
            extents_object["x"] = SerializeFloat(this.extents.x);
            extents_object["y"] = SerializeFloat(this.extents.y);
            extents_object["z"] = SerializeFloat(this.extents.z);

            result["center"] = center_object;
            result["orientation"] = orientation_object;
            result["extents"] = extents_object;

            return result;
        }
    }
}