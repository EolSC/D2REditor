using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class ModelVariationComponent : LevelEntityComponent
    {
        public float weight = 0.0f;
        public ModelDefinitionComponent model;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            weight = ISerializable.DeserializeFloat(json["weight"]);
            model = gameObject.AddComponent<ModelDefinitionComponent>();
            model.Deserialize(json);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["weight"] = ISerializable.SerializeFloat(weight);
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