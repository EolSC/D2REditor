using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Mesh collider
    */
    public class PhysicsFileDefinition : PhysicsShapeType
    {
        public string filename;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            filename = obj["filename"];
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            return result;
        }
    }
}