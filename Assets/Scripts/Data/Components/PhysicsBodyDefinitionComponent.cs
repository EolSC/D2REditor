using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
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
            fixturedefs = DeserializeList<PhysicsFixture>(json, "fixturedefs");
            filter = json["filter"];
            allowTransition = json["allowTransition"];
            removeOnDeath = json["removeOnDeath"];

            lineardamping = DeserializeFloat(json["lineardamping"]);
            angulardamping = DeserializeFloat(json["angulardamping"]);
            gravityscale = DeserializeFloat(json["gravityscale"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["bodytype"] = bodytype;
            result["fixturedefs"] = SerializeList(fixturedefs);

            result["filter"] = filter;
            result["allowTransition"] = allowTransition;
            result["removeOnDeath"] = removeOnDeath;

            result["lineardamping"] = SerializeFloat(lineardamping);
            result["angulardamping"] = SerializeFloat(angulardamping);
            result["gravityscale"] = SerializeFloat(gravityscale);
            return result;
        }
    }
}