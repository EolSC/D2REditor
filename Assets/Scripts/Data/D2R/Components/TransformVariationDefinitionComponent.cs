using SimpleJSON;
using System;
using UnityEngine;
namespace Diablo2Editor
{
    public class TransformVariationDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 scale;

        public UnityEngine.Vector3 minRotation;
        public UnityEngine.Vector3 maxRotation;
        public UnityEngine.Vector3 rotationIncrement;
        public UnityEngine.Vector3 minScale;
        public UnityEngine.Vector3 maxScale;
        public UnityEngine.Vector3 scaleIncrement;

        private void UpdateObjectTransform()
        {
            gameObject.transform.SetLocalPositionAndRotation(this.position, this.orientation);
            gameObject.transform.localScale = this.scale;
        }
        public override void Instantiate()
        {
            base.Instantiate();
            UpdateObjectTransform();
        }

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject position = obj["position"].AsObject;
            this.position.x = ISerializable.DeserializeFloat(position["x"]);
            this.position.y = ISerializable.DeserializeFloat(position["y"]);
            this.position.z = ISerializable.DeserializeFloat(position["z"]);

            JSONObject minRotation = obj["minRotation"].AsObject;
            this.minRotation.x = ISerializable.DeserializeFloat(minRotation["x"]);
            this.minRotation.y = ISerializable.DeserializeFloat(minRotation["y"]);
            this.minRotation.z = ISerializable.DeserializeFloat(minRotation["z"]);

            JSONObject maxRotation = obj["maxRotation"].AsObject;
            this.maxRotation.x = ISerializable.DeserializeFloat(maxRotation["x"]);
            this.maxRotation.y = ISerializable.DeserializeFloat(maxRotation["y"]);
            this.maxRotation.z = ISerializable.DeserializeFloat(maxRotation["z"]);

            JSONObject rotationIncrement = obj["rotationIncrement"].AsObject;
            this.rotationIncrement.x = ISerializable.DeserializeFloat(rotationIncrement["x"]);
            this.rotationIncrement.y = ISerializable.DeserializeFloat(rotationIncrement["y"]);
            this.rotationIncrement.z = ISerializable.DeserializeFloat(rotationIncrement["z"]);

            JSONObject minScale = obj["minScale"].AsObject;
            this.minScale.x = ISerializable.DeserializeFloat(minScale["x"]);
            this.minScale.y = ISerializable.DeserializeFloat(minScale["y"]);
            this.minScale.z = ISerializable.DeserializeFloat(minScale["z"]);

            JSONObject maxScale = obj["maxScale"].AsObject;
            this.maxScale.x = ISerializable.DeserializeFloat(maxScale["x"]);
            this.maxScale.y = ISerializable.DeserializeFloat(maxScale["y"]);
            this.maxScale.z = ISerializable.DeserializeFloat(maxScale["z"]);

            JSONObject scaleIncrement = obj["scaleIncrement"].AsObject;
            this.scaleIncrement.x = ISerializable.DeserializeFloat(scaleIncrement["x"]);
            this.scaleIncrement.y = ISerializable.DeserializeFloat(scaleIncrement["y"]);
            this.scaleIncrement.z = ISerializable.DeserializeFloat(scaleIncrement["z"]);

            RandomizeTransform();
            UpdateObjectTransform();
        }

        private void RandomizeTransform()
        {
            this.scale = RandomizeVector(minScale, maxScale, scaleIncrement);
            Vector3 rotation = RandomizeVector(minRotation, maxRotation, rotationIncrement);
            orientation.x = rotation.x;
            orientation.y = rotation.y;
            orientation.z = rotation.z;
            orientation.w = 1.0f;
        }

        private Vector3 RandomizeVector(Vector3 min, Vector3 max, Vector3 step)
        {
            float x = RandomizeFloat(min.x, max.x, step.x);
            float y = RandomizeFloat(min.y, max.y, step.y);
            float z = RandomizeFloat(min.z, max.z, step.z);

            return new Vector3(x, y, z);
        }

        private float RandomizeFloat(float min, float max, float step)
        {
            if (max < min)
            {
                float temp = min;
                min = max;
                max = temp;
            }
            if (step > 0)
            {
                int stepCount = (int)Math.Floor(max - min/step);
                float value = Math.Clamp(min + UnityEngine.Random.Range(0, stepCount) * step, min, max);
                return value;
            }
            return min;
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject pos_object = new JSONObject();
            pos_object["x"] = ISerializable.SerializeFloat(this.position.x);
            pos_object["y"] = ISerializable.SerializeFloat(this.position.y);
            pos_object["z"] = ISerializable.SerializeFloat(this.position.z);
            result["position"] = pos_object;

            JSONObject min_rotatation_object = new JSONObject();
            min_rotatation_object["x"] = ISerializable.SerializeFloat(this.minRotation.x);
            min_rotatation_object["y"] = ISerializable.SerializeFloat(this.minRotation.y);
            min_rotatation_object["z"] = ISerializable.SerializeFloat(this.minRotation.z);
            result["minRotation"] = min_rotatation_object;

            JSONObject max_rotation_object = new JSONObject();
            max_rotation_object["x"] = ISerializable.SerializeFloat(this.maxRotation.x);
            max_rotation_object["y"] = ISerializable.SerializeFloat(this.maxRotation.y);
            max_rotation_object["z"] = ISerializable.SerializeFloat(this.maxRotation.z);
            result["maxRotation"] = max_rotation_object;

            JSONObject rotation_increment_object = new JSONObject();
            rotation_increment_object["x"] = ISerializable.SerializeFloat(this.rotationIncrement.x);
            rotation_increment_object["y"] = ISerializable.SerializeFloat(this.rotationIncrement.y);
            rotation_increment_object["z"] = ISerializable.SerializeFloat(this.rotationIncrement.z);
            result["rotationIncrement"] = rotation_increment_object;

            JSONObject min_scale_object = new JSONObject();
            min_scale_object["x"] = ISerializable.SerializeFloat(this.minScale.x);
            min_scale_object["y"] = ISerializable.SerializeFloat(this.minScale.y);
            min_scale_object["z"] = ISerializable.SerializeFloat(this.minScale.z);
            result["minScale"] = min_scale_object;

            JSONObject max_scale_object = new JSONObject();
            max_scale_object["x"] = ISerializable.SerializeFloat(this.maxScale.x);
            max_scale_object["y"] = ISerializable.SerializeFloat(this.maxScale.y);
            max_scale_object["z"] = ISerializable.SerializeFloat(this.maxScale.z);
            result["maxScale"] = max_scale_object;

            JSONObject scale_increment_object = new JSONObject();
            scale_increment_object["x"] = ISerializable.SerializeFloat(this.scaleIncrement.x);
            scale_increment_object["y"] = ISerializable.SerializeFloat(this.scaleIncrement.y);
            scale_increment_object["z"] = ISerializable.SerializeFloat(this.scaleIncrement.z);
            result["scaleIncrement"] = scale_increment_object;


            return result;
        }
    }
}
