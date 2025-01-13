using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
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


        private PhysicsShapeType LoadShape(JSONObject json)
        {
            string type = json["type"];
            if (type == "PhysicsBoxDefinition")
            {
                PhysicsShapeType box = new PhysicsShapeBox();
                box.Deserialize(json);
                return box;
            }
            if (type == "PhysicsFileDefinition")
            {
                PhysicsFileDefinition file = new PhysicsFileDefinition();
                file.Deserialize(json);
                return file;
            }
            return new PhysicsShapeType();

        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject shape_object = json["shapetype"].AsObject;

            shapeType = LoadShape(shape_object);
            friction = DeserializeFloat(json["friction"]);
            restitution = DeserializeFloat(json["restitution"]);
            rollingresistance = DeserializeFloat(json["rollingresistance"]);
            impulsefactor = DeserializeFloat(json["impulsefactor"]);
            explosionfactor = DeserializeFloat(json["explosionfactor"]);
            explosionliftfactor = DeserializeFloat(json["explosionliftfactor"]);
            windfactor = DeserializeFloat(json["windfactor"]);
            dragfactor = DeserializeFloat(json["dragfactor"]);
            liftfactor = DeserializeFloat(json["liftfactor"]);
            density = DeserializeFloat(json["density"]);
            boneindex = json["boneindex"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["shapetype"] = shapeType.Serialize();
            result["friction"] = SerializeFloat(friction);
            result["restitution"] = SerializeFloat(restitution);
            result["rollingresistance"] = SerializeFloat(rollingresistance);
            result["impulsefactor"] = SerializeFloat(impulsefactor);
            result["explosionfactor"] = SerializeFloat(explosionfactor);
            result["explosionliftfactor"] = SerializeFloat(explosionliftfactor);
            result["windfactor"] = SerializeFloat(windfactor);
            result["dragfactor"] = SerializeFloat(dragfactor);
            result["liftfactor"] = SerializeFloat(liftfactor);
            result["density"] = SerializeFloat(density);
            result["boneindex"] = boneindex;
            return result;
        }
    }
}