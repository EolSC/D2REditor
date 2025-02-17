using SimpleJSON;
using UnityEngine;

namespace Diablo2Editor
{
    public class SkeletonDefinitionComponent : LevelEntityComponent
    {
        public string filename;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            filename = json["filename"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            return result;
        }
    }

}
