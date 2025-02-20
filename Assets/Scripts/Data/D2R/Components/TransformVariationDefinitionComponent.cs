using SimpleJSON;
using System;
using UnityEngine;
namespace Diablo2Editor
{
    public class TransformVariationDefinitionComponent : LevelEntityComponent
    {
        public Vector3 position;
        public Quaternion orientation;
        public Vector3 scale;
            
        public Vector3 minRotation;
        public Vector3 maxRotation;
        public Vector3 rotationIncrement;
        public Vector3 minScale;
        public Vector3 maxScale;
        public Vector3 scaleIncrement;

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

        private void SaveCurrentTrasform()
        {
            position = gameObject.transform.position;
            position.x = -position.x;
        }

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            this.position = ISerializable.DeserializeVector(obj["position"].AsObject);

            this.minRotation = ISerializable.DeserializeVector(obj["minRotation"].AsObject);
            this.maxRotation = ISerializable.DeserializeVector(obj["maxRotation"].AsObject);
            this.rotationIncrement = ISerializable.DeserializeVector(obj["rotationIncrement"].AsObject);

            this.minScale = ISerializable.DeserializeVector(obj["minScale"].AsObject);
            this.maxScale = ISerializable.DeserializeVector(obj["maxScale"].AsObject);
            this.scaleIncrement = ISerializable.DeserializeVector(obj["scaleIncrement"].AsObject);

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

            SaveCurrentTrasform();
            result["position"] = ISerializable.SerializeVector(this.position);

            result["minRotation"] = ISerializable.SerializeVector(this.minRotation);
            result["maxRotation"] = ISerializable.SerializeVector(this.maxRotation);
            result["rotationIncrement"] = ISerializable.SerializeVector(this.rotationIncrement);

            result["minScale"] = ISerializable.SerializeVector(this.minScale);
            result["maxScale"] = ISerializable.SerializeVector(this.maxScale);
            result["scaleIncrement"] = ISerializable.SerializeVector(this.scaleIncrement);

            return result;
        }
    }
}
