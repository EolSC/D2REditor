using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Transform component. Passes transform data to Unity via gameObject.transform
    * 
    */
    public class TransformDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 scale;
        public bool inheritOnlyPosition = true;
        private void UpdateTransform()
        {
            gameObject.transform.SetLocalPositionAndRotation(this.position, this.orientation);
            gameObject.transform.localScale = this.scale;
        }

        public override void Instantiate()
        {
            base.Instantiate();
            UpdateTransform();
        }

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject position = obj["position"].AsObject;
            this.position.x = ISerializable.DeserializeFloat(position["x"]);
            this.position.y = ISerializable.DeserializeFloat(position["y"]);
            this.position.z = ISerializable.DeserializeFloat(position["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = ISerializable.DeserializeFloat(orientation["x"]);
            this.orientation.y = ISerializable.DeserializeFloat(orientation["y"]);
            this.orientation.z = ISerializable.DeserializeFloat(orientation["z"]);
            this.orientation.w = ISerializable.DeserializeFloat(orientation["w"]);

            JSONObject scale = obj["scale"].AsObject;
            this.scale.x = ISerializable.DeserializeFloat(scale["x"]);
            this.scale.y = ISerializable.DeserializeFloat(scale["y"]);
            this.scale.z = ISerializable.DeserializeFloat(scale["z"]);

            this.inheritOnlyPosition = obj["inheritOnlyPosition"].AsBool;
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject pos_object = new JSONObject();
            pos_object["x"] = ISerializable.SerializeFloat(this.position.x);
            pos_object["y"] = ISerializable.SerializeFloat(this.position.y);
            pos_object["z"] = ISerializable.SerializeFloat(this.position.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = ISerializable.SerializeFloat(this.orientation.x);
            orientation_object["y"] = ISerializable.SerializeFloat(this.orientation.y);
            orientation_object["z"] = ISerializable.SerializeFloat(this.orientation.z);
            orientation_object["w"] = ISerializable.SerializeFloat(this.orientation.w);

            JSONObject scale_object = new JSONObject();
            scale_object["x"] = ISerializable.SerializeFloat(this.scale.x);
            scale_object["y"] = ISerializable.SerializeFloat(this.scale.y);
            scale_object["z"] = ISerializable.SerializeFloat(this.scale.z);

            result["position"] = pos_object;
            result["orientation"] = orientation_object;
            result["scale"] = scale_object;
            result["inheritOnlyPosition"] = this.inheritOnlyPosition;

            return result;
        }
    }
}