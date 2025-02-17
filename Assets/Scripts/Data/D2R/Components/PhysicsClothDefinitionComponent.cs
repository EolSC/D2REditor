using SimpleJSON;
using UnityEngine;
namespace Diablo2Editor
{
    public class PhysicsClothDefinitionComponent : LevelEntityComponent
    {
        public float actorTrackingFactor = 0.0f;
        public float bendStiffness = 0.0f;
        public float blendDamping = 0.0f;
        public float blendHz = 0.0f;
        public float boneTrackingFactor = 0.0f;
        public float dampingFactor = 0.0f;
        public float dragFactor = 0.0f;
        public float explosionFactor = 0.0f;
        public Vector3 gravity;
        public float impulseFactor = 0.0f;
        public bool isStatic =false;
        public float liftFactor = 0.0f;
        public int minimumPlatformTier = 0;
        public Vector3 selfWind;
        public float shearStiffness = 0.0f;
        public float skinStiffness = 0.0f;
        public float warpStiffness = 0.0f;
        public float weftStiffness = 0.0f;
        public float windFactor = 0.0f;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            actorTrackingFactor = ISerializable.DeserializeFloat(json["actorTrackingFactor"]);
            bendStiffness = ISerializable.DeserializeFloat(json["bendStiffness"]);
            blendDamping = ISerializable.DeserializeFloat(json["blendDamping"]);
            blendHz = ISerializable.DeserializeFloat(json["blendHz"]);
            boneTrackingFactor = ISerializable.DeserializeFloat(json["boneTrackingFactor"]);
            dampingFactor = ISerializable.DeserializeFloat(json["dampingFactor"]);
            dragFactor = ISerializable.DeserializeFloat(json["dragFactor"]);
            explosionFactor = ISerializable.DeserializeFloat(json["explosionFactor"]);
            
            gravity = ISerializable.DeserializeVector(json["gravity"].AsObject);
            impulseFactor = ISerializable.DeserializeFloat(json["impulseFactor"]);
            isStatic = json["isStatic"];

            liftFactor = ISerializable.DeserializeFloat(json["liftFactor"]);
            minimumPlatformTier = json["minimumPlatformTier"];
            
            selfWind = ISerializable.DeserializeVector(json["selfWind"].AsObject);

            shearStiffness = ISerializable.DeserializeFloat(json["shearStiffness"]);
            skinStiffness = ISerializable.DeserializeFloat(json["skinStiffness"]);
            warpStiffness = ISerializable.DeserializeFloat(json["warpStiffness"]);
            weftStiffness = ISerializable.DeserializeFloat(json["weftStiffness"]);
            windFactor = ISerializable.DeserializeFloat(json["windFactor"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["actorTrackingFactor"] = ISerializable.SerializeFloat(actorTrackingFactor);
            result["bendStiffness"] = ISerializable.SerializeFloat(bendStiffness);
            result["blendDamping"] = ISerializable.SerializeFloat(blendDamping);
            result["blendHz"] = ISerializable.SerializeFloat(blendHz);
            result["boneTrackingFactor"] = ISerializable.SerializeFloat(boneTrackingFactor);
            result["dampingFactor"] = ISerializable.SerializeFloat(dampingFactor);
            result["dragFactor"] = ISerializable.SerializeFloat(dragFactor);
            result["explosionFactor"] = ISerializable.SerializeFloat(explosionFactor);

            result["gravity"] = ISerializable.SerializeVector(gravity);

            result["impulseFactor"] = ISerializable.SerializeFloat(impulseFactor);
            result["isStatic"] = isStatic;
            result["liftFactor"] = ISerializable.SerializeFloat(liftFactor);
            result["minimumPlatformTier"] = minimumPlatformTier;
            result["selfWind"] = ISerializable.SerializeVector(selfWind);

            result["shearStiffness"] = ISerializable.SerializeFloat(shearStiffness);
            result["skinStiffness"] = ISerializable.SerializeFloat(skinStiffness);
            result["warpStiffness"] = ISerializable.SerializeFloat(warpStiffness);
            result["weftStiffness"] = ISerializable.SerializeFloat(weftStiffness);
            result["windFactor"] = ISerializable.SerializeFloat(windFactor);

            return result;
        }

    }

}
