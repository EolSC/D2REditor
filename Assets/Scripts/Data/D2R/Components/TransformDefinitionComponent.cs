using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Transform component. Passes transform data to Unity via gameObject.transform
    * 
    */
    [ExecuteInEditMode]
    public class TransformDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 scale;
        public bool inheritOnlyPosition = true;
        private void UpdateObjectTransform()
        {
            gameObject.transform.SetLocalPositionAndRotation(this.position, this.orientation);
            gameObject.transform.localScale = this.scale;
        }
        void Update()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                SaveCurrentTrasform();
            }
        }

        private void SaveCurrentTrasform()
        {
            position = gameObject.transform.position;
            position.x = -position.x;
            orientation = gameObject.transform.localRotation;
            scale = gameObject.transform.localScale;
        }

        public override void Instantiate()
        {
            base.Instantiate();
            UpdateObjectTransform();
        }

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            this.position = ISerializable.DeserializeVector(obj["position"].AsObject);
            this.scale = ISerializable.DeserializeVector(obj["scale"].AsObject);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = ISerializable.DeserializeFloat(orientation["x"]);
            this.orientation.y = ISerializable.DeserializeFloat(orientation["y"]);
            this.orientation.z = ISerializable.DeserializeFloat(orientation["z"]);
            this.orientation.w = ISerializable.DeserializeFloat(orientation["w"]);

            this.inheritOnlyPosition = obj["inheritOnlyPosition"].AsBool;

            UpdateObjectTransform();
        }
        public override JSONObject Serialize()
        {
            SaveCurrentTrasform();

            JSONObject result = base.Serialize();

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = ISerializable.SerializeFloat(this.orientation.x);
            orientation_object["y"] = ISerializable.SerializeFloat(this.orientation.y);
            orientation_object["z"] = ISerializable.SerializeFloat(this.orientation.z);
            orientation_object["w"] = ISerializable.SerializeFloat(this.orientation.w);

            result["position"] = ISerializable.SerializeVector(position);
            result["scale"] = ISerializable.SerializeVector(scale);
            result["orientation"] = orientation_object;
            result["inheritOnlyPosition"] = this.inheritOnlyPosition;

            return result;
        }
    }
}