using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Box collider
    */

    public class PhysicsShapeBox : PhysicsShapeType
    {
        public UnityEngine.Vector3 center;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 extents;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);

            this.center = ISerializable.DeserializeVector(obj["center"].AsObject);
            this.extents = ISerializable.DeserializeVector(obj["extents"].AsObject);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = ISerializable.DeserializeFloat(orientation["x"]);
            this.orientation.y = ISerializable.DeserializeFloat(orientation["y"]);
            this.orientation.z = ISerializable.DeserializeFloat(orientation["z"]);
            this.orientation.w = ISerializable.DeserializeFloat(orientation["w"]);

        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = ISerializable.SerializeFloat(this.orientation.x);
            orientation_object["y"] = ISerializable.SerializeFloat(this.orientation.y);
            orientation_object["z"] = ISerializable.SerializeFloat(this.orientation.z);
            orientation_object["w"] = ISerializable.SerializeFloat(this.orientation.w);
            result["orientation"] = orientation_object;


            result["center"] = ISerializable.SerializeVector(this.center);
            result["extents"] = ISerializable.SerializeVector(this.extents);

            return result;
        }
    }
}