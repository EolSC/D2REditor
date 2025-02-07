using SimpleJSON;
namespace Diablo2Editor
{
    public class OverrideModelDefinitionComponent : LevelEntityComponent
    {
        public int regionsToOverride;
        public string filename;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            regionsToOverride = json["regionsToOverride"];
            filename = json["filename"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["regionsToOverride"] = regionsToOverride;
            result["filename"] = filename;
            return result;
        }
    }

}
