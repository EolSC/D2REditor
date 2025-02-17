using NUnit.Framework;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
namespace Diablo2Editor
{

    public class JointSettings : LevelEntityComponent
    {
        public float angularDampingRatio;
        public float angularFrequencyHz;
        public string bodyType;
        public string boneName;
        int categoryBits;
        public float coneAngle;
        public float density;
        public float explosionFactor;
        public float explosionLiftFactor;
        public float friction;
        public float frictionTorque;
        public int includeMask;
        public float jointFriction;
        public int jointType;
        public float linearDampingRatio;
        public float linearFrequencyHz;
        public float lowerTwistAngle;
        public float motorDampingRatio;
        public float motorFrequencyHz;
        public float radius;
        public float restitution;
        public float rollingResistance;
        public float upperTwistAngle;
        public float windDragFactor;
        public float windFactor;
        public float windLiftFactor;


        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            angularDampingRatio = ISerializable.DeserializeFloat(json["angularDampingRatio"]);
            angularFrequencyHz = ISerializable.DeserializeFloat(json["angularFrequencyHz"]);

            bodyType = json["bodyType"];
            boneName = json["boneName"];

            categoryBits = json["categoryBits"];

            coneAngle = ISerializable.DeserializeFloat(json["coneAngle"]);
            density = ISerializable.DeserializeFloat(json["density"]);
            explosionFactor = ISerializable.DeserializeFloat(json["explosionFactor"]);
            explosionLiftFactor = ISerializable.DeserializeFloat(json["explosionLiftFactor"]);
            friction = ISerializable.DeserializeFloat(json["friction"]);
            frictionTorque = ISerializable.DeserializeFloat(json["frictionTorque"]);
            

            includeMask = json["includeMask"];
            jointFriction = ISerializable.DeserializeFloat(json["jointFriction"]);
            jointType = json["jointType"];

            linearDampingRatio = ISerializable.DeserializeFloat(json["linearDampingRatio"]);
            linearFrequencyHz = ISerializable.DeserializeFloat(json["linearFrequencyHz"]);
            lowerTwistAngle = ISerializable.DeserializeFloat(json["lowerTwistAngle"]);
            motorDampingRatio = ISerializable.DeserializeFloat(json["motorDampingRatio"]);
            motorFrequencyHz = ISerializable.DeserializeFloat(json["motorFrequencyHz"]);

            radius = ISerializable.DeserializeFloat(json["radius"]);
            restitution = ISerializable.DeserializeFloat(json["restitution"]);
            rollingResistance = ISerializable.DeserializeFloat(json["rollingResistance"]);

            upperTwistAngle = ISerializable.DeserializeFloat(json["upperTwistAngle"]);

            windDragFactor = ISerializable.DeserializeFloat(json["windDragFactor"]);
            windFactor = ISerializable.DeserializeFloat(json["windFactor"]);
            windLiftFactor = ISerializable.DeserializeFloat(json["windLiftFactor"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["angularDampingRatio"] = ISerializable.SerializeFloat(angularDampingRatio);
            result["angularFrequencyHz"] = ISerializable.SerializeFloat(angularFrequencyHz);

            result["bodyType"] = bodyType;
            result["boneName"] = boneName;

            result["categoryBits"] = categoryBits;

            result["coneAngle"] = ISerializable.SerializeFloat(coneAngle);
            result["density"] = ISerializable.SerializeFloat(density);
            result["explosionFactor"] = ISerializable.SerializeFloat(explosionFactor);
            result["explosionLiftFactor"] = ISerializable.SerializeFloat(explosionLiftFactor);
            result["friction"] = ISerializable.SerializeFloat(friction);
            result["frictionTorque"] = ISerializable.SerializeFloat(frictionTorque);

            result["includeMask"] = includeMask;
            result["jointFriction"] = ISerializable.SerializeFloat(jointFriction);
            result["jointType"] = jointType;

            result["linearDampingRatio"] = ISerializable.SerializeFloat(linearDampingRatio);
            result["linearFrequencyHz"] = ISerializable.SerializeFloat(linearFrequencyHz);
            result["lowerTwistAngle"] = ISerializable.SerializeFloat(lowerTwistAngle);

            result["motorDampingRatio"] = ISerializable.SerializeFloat(motorDampingRatio);
            result["motorFrequencyHz"] = ISerializable.SerializeFloat(motorFrequencyHz);

            result["radius"] = ISerializable.SerializeFloat(radius);
            result["restitution"] = ISerializable.SerializeFloat(restitution);
            result["rollingResistance"] = ISerializable.SerializeFloat(rollingResistance);

            result["upperTwistAngle"] = ISerializable.SerializeFloat(upperTwistAngle);

            result["windDragFactor"] = ISerializable.SerializeFloat(windDragFactor);
            result["windFactor"] = ISerializable.SerializeFloat(windFactor);
            result["windLiftFactor"] = ISerializable.SerializeFloat(windLiftFactor);

            return result;
        }
    }

    public class PhysicsPartialRagdollDefinitionComponent : LevelEntityComponent
    {
        public Vector3 anchorLocation;
        public Vector3 attachLocation;
        public float density;
        public List<JointSettings> jointSettings = new List<JointSettings>();
        public float maxWindSpeed;
        public int minimumPlatformTier = 0;
        public int positionIterations = 0;
        public float radius = 0.0f;
        public float tracking = 0.0f;
        public int velocityIterations = 0;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            anchorLocation = ISerializable.DeserializeVector(json["anchorLocation"].AsObject);
            attachLocation = ISerializable.DeserializeVector(json["attachLocation"].AsObject);
            density = ISerializable.DeserializeFloat(json["density"]);
            jointSettings = ISerializable.DeserializeComponentList<JointSettings>(json, this.entity, "jointSettings");

            maxWindSpeed = ISerializable.DeserializeFloat(json["maxWindSpeed"]);
            minimumPlatformTier = json["minimumPlatformTier"];
            positionIterations = json["positionIterations"];
            radius = ISerializable.DeserializeFloat(json["radius"]);
            tracking = ISerializable.DeserializeFloat(json["tracking"]);
            velocityIterations = json["velocityIterations"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["anchorLocation"] = ISerializable.SerializeVector(anchorLocation);
            result["attachLocation"] = ISerializable.SerializeVector(attachLocation);

            result["density"] = ISerializable.SerializeFloat(density);
            result["jointSettings"] = ISerializable.SerializeList(jointSettings);

            result["maxWindSpeed"] = ISerializable.SerializeFloat(maxWindSpeed);
            result["minimumPlatformTier"] = minimumPlatformTier;
            result["positionIterations"] = positionIterations;
            result["radius"] = ISerializable.SerializeFloat(radius);
            result["tracking"] = ISerializable.SerializeFloat(tracking);
            result["velocityIterations"] = velocityIterations;

            return result;
        }
    }
}
