using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class ModelVariationComponent : LevelEntityComponent
    {
        public float weight = 0.0f;
        public ModelDefinitionComponent model = new ModelDefinitionComponent();

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            weight = DeserializeFloat(json["weight"]);
            model.Deserialize(json);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["weight"] = SerializeFloat(weight);
            result["filename"] = model.filename;
            result["visibleLayers"] = model.visibleLayers;
            result["lightMask"] = model.lightMask;
            result["shadowMask"] = model.shadowMask;
            result["ghostShadows"] = model.ghostShadows;
            result["floorModel"] = model.floorModel;
            result["terrainBlendEnableYUpBlend"] = model.terrainBlendEnableYUpBlend;
            result["terrainBlendMode"] = model.terrainBlendMode;
            return result;
        }

    }

}