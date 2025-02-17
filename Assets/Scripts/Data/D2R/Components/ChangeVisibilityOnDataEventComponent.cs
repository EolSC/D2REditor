using SimpleJSON;
namespace Diablo2Editor
{
    public class ChangeVisibilityOnDataEventComponent : LevelEntityComponent
    {
        public string eventName;
        int popGroup = -1;
        bool visibleOnEvent = false;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            eventName = json["eventName"];
            popGroup = json["popGroup"];
            visibleOnEvent = json["visibleOnEvent"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["eventName"] = eventName;
            result["popGroup"] = popGroup;
            result["visibleOnEvent"] = visibleOnEvent;

            return result;
        }
    }
}

