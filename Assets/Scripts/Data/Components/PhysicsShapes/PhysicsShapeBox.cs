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
            this.center.x = ISerializable.DeserializeFloat(center["x"]);
            this.center.y = ISerializable.DeserializeFloat(center["y"]);
            this.center.z = ISerializable.DeserializeFloat(center["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = ISerializable.DeserializeFloat(orientation["x"]);
            this.orientation.y = ISerializable.DeserializeFloat(orientation["y"]);
            this.orientation.z = ISerializable.DeserializeFloat(orientation["z"]);
            this.orientation.w = ISerializable.DeserializeFloat(orientation["w"]);

            JSONObject extents = obj["extents"].AsObject;
            this.extents.x = ISerializable.DeserializeFloat(extents["x"]);
            this.extents.y = ISerializable.DeserializeFloat(extents["y"]);
            this.extents.z = ISerializable.DeserializeFloat(extents["z"]);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject center_object = new JSONObject();
            center_object["x"] = ISerializable.SerializeFloat(this.center.x);
            center_object["y"] = ISerializable.SerializeFloat(this.center.y);
            center_object["z"] = ISerializable.SerializeFloat(this.center.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = ISerializable.SerializeFloat(this.orientation.x);
            orientation_object["y"] = ISerializable.SerializeFloat(this.orientation.y);
            orientation_object["z"] = ISerializable.SerializeFloat(this.orientation.z);
            orientation_object["w"] = ISerializable.SerializeFloat(this.orientation.w);

            JSONObject extents_object = new JSONObject();
            extents_object["x"] = ISerializable.SerializeFloat(this.extents.x);
            extents_object["y"] = ISerializable.SerializeFloat(this.extents.y);
            extents_object["z"] = ISerializable.SerializeFloat(this.extents.z);

            result["center"] = center_object;
            result["orientation"] = orientation_object;
            result["extents"] = extents_object;

            return result;
        }
    }
}