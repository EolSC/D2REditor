using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class PrefabPlacementDefinitionComponent : LevelEntityComponent
    {
        public string prefab;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            prefab = json["prefab"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["prefab"] = prefab;
            return result;
        }
    }
}

