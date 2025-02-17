using SimpleJSON;
namespace Diablo2Editor
{
    public class DefaultAttachmentTransformComponent : LevelEntityComponent
    {
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["defaultTransforms"] = new JSONObject();

            return result;
        }
    }

}

