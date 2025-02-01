using SimpleJSON;

namespace Diablo2Editor
{
    public class ModelPlatformTierComponent : LevelEntityComponent
    {
        public int visibilityTier;
        public int shadowTier;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            visibilityTier = json["visibilityTier"];
            shadowTier = json["shadowTier"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["visibilityTier"] = visibilityTier;
            result["shadowTier"] = shadowTier;
            return result;
        }
    }
}