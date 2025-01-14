using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    public class ModelVariationDefinitionComponent : LevelEntityComponent
    {
        public List<ModelVariationComponent> variations;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            variations = ISerializable.DeserializeComponentList<ModelVariationComponent>(json, gameObject, "variations");
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["variations"] = ISerializable.SerializeList(variations);
            return result;
        }
    }
}