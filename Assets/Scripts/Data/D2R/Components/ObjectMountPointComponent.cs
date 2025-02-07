using SimpleJSON;

namespace Diablo2Editor
{
    public class ObjectMountPointComponent : LevelEntityComponent
    {
        public long mountTarget;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            mountTarget = json["mountTarget"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["mountTarget"] = mountTarget;
            return result;
        }
    }

}
