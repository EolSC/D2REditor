using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{

    public class LevelEntityComponent : MonoBehaviour, ISerializable
    {
        public string component_type;
        public string component_name;

        public virtual void Deserialize(JSONObject json)
        {
            component_type = json["type"];
            component_name = json["name"];
        }

        public virtual void OnLoaded()
        {

        }

        public virtual JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = component_type;
            result["name"] = component_name;
            return result;
        }

    }
}