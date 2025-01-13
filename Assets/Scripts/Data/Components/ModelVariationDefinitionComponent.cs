using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    public class ModelVariationDefinitionComponent : LevelEntityComponent
    {
        public List<ModelVariationComponent> variations = new List<ModelVariationComponent>();
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            variations = DeserializeList<ModelVariationComponent>(json, "variations");
        }

        public override JSONObject Serialize()
        {
            // TODO - весериализовать базу, вес, моедль именно в таком порядке
            JSONObject result = base.Serialize();
            result["variations"] = SerializeList(variations);
            return result;
        }
    }
}