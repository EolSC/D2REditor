using SimpleJSON;

namespace Diablo2Editor
{
    public class UnitPartComponent : LevelEntityComponent
    {
        public string part;
        public string variant;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            part = json["part"];
            variant = json["variant"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["part"] = part;
            result["variant"] = variant;
            return result;
        }
    }

}
