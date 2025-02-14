using SimpleJSON;
using UnityEngine;
namespace Diablo2Editor
{
    public class FadeTargetComponent : LevelEntityComponent
    {
        public int targetId = 0;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            targetId = json["targetId"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["targetId"] = targetId;

            return result;
        }
    }

}
