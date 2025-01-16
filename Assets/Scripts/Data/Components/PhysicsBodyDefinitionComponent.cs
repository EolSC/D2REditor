using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    /*
     * Physics body component. Used for object physics.
     * 
     */
    public class PhysicsBodyDefinitionComponent : LevelEntityComponent
    {
        public string bodytype;
        public List<PhysicsFixture> fixturedefs;
        public string filter;
        public bool allowTransition = true;
        public bool removeOnDeath = true;
        public float lineardamping = 0;
        public float angulardamping = 0;
        public float gravityscale = 0;




        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            bodytype = json["bodytype"];
            fixturedefs = ISerializable.DeserializeComponentList<PhysicsFixture>(json, gameObject, "fixturedefs");
            filter = json["filter"];
            allowTransition = json["allowTransition"];
            removeOnDeath = json["removeOnDeath"];

            lineardamping = ISerializable.DeserializeFloat(json["lineardamping"]);
            angulardamping = ISerializable.DeserializeFloat(json["angulardamping"]);
            gravityscale = ISerializable.DeserializeFloat(json["gravityscale"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["bodytype"] = bodytype;
            result["fixturedefs"] = ISerializable.SerializeList(fixturedefs);

            result["filter"] = filter;
            result["allowTransition"] = allowTransition;
            result["removeOnDeath"] = removeOnDeath;

            result["lineardamping"] = ISerializable.SerializeFloat(lineardamping);
            result["angulardamping"] = ISerializable.SerializeFloat(angulardamping);
            result["gravityscale"] = ISerializable.SerializeFloat(gravityscale);
            return result;
        }
    }
}