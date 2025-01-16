using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
   * Physics fixture component. Used for object physics.
   * 
   */
    public class PhysicsFixture : LevelEntityComponent
    {
        public PhysicsShapeType shapeType = null;
        public float friction = 0.0f;
        public float restitution = 0.0f;
        public float rollingresistance = 0.0f;
        public float impulsefactor = 0.0f;
        public float explosionfactor = 0.0f;
        public float explosionliftfactor = 0.0f;
        public float windfactor = 0.0f;
        public float dragfactor = 0.0f;
        public float liftfactor = 0.0f;
        public float density = 0.0f;
        public int boneindex = -1;


        private PhysicsShapeType LoadShape(JSONObject json , GameObject owner)
        {
            string type = json["type"];
            if (type == "PhysicsBoxDefinition")
            {
                PhysicsShapeType box = owner.AddComponent<PhysicsShapeBox>();
                box.Deserialize(json);
                return box;
            }
            if (type == "PhysicsFileDefinition")
            {
                PhysicsFileDefinition file = owner.AddComponent<PhysicsFileDefinition>();
                file.Deserialize(json);
                return file;
            }
            return owner.AddComponent<PhysicsShapeType>();

        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject shape_object = json["shapetype"].AsObject;

            shapeType = LoadShape(shape_object, gameObject);
            friction = ISerializable.DeserializeFloat(json["friction"]);
            restitution = ISerializable.DeserializeFloat(json["restitution"]);
            rollingresistance = ISerializable.DeserializeFloat(json["rollingresistance"]);
            impulsefactor = ISerializable.DeserializeFloat(json["impulsefactor"]);
            explosionfactor = ISerializable.DeserializeFloat(json["explosionfactor"]);
            explosionliftfactor = ISerializable.DeserializeFloat(json["explosionliftfactor"]);
            windfactor = ISerializable.DeserializeFloat(json["windfactor"]);
            dragfactor = ISerializable.DeserializeFloat(json["dragfactor"]);
            liftfactor = ISerializable.DeserializeFloat(json["liftfactor"]);
            density = ISerializable.DeserializeFloat(json["density"]);
            boneindex = json["boneindex"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["shapetype"] = shapeType.Serialize();
            result["friction"] = ISerializable.SerializeFloat(friction);
            result["restitution"] = ISerializable.SerializeFloat(restitution);
            result["rollingresistance"] = ISerializable.SerializeFloat(rollingresistance);
            result["impulsefactor"] = ISerializable.SerializeFloat(impulsefactor);
            result["explosionfactor"] = ISerializable.SerializeFloat(explosionfactor);
            result["explosionliftfactor"] = ISerializable.SerializeFloat(explosionliftfactor);
            result["windfactor"] = ISerializable.SerializeFloat(windfactor);
            result["dragfactor"] = ISerializable.SerializeFloat(dragfactor);
            result["liftfactor"] = ISerializable.SerializeFloat(liftfactor);
            result["density"] = ISerializable.SerializeFloat(density);
            result["boneindex"] = boneindex;
            return result;
        }
    }
}