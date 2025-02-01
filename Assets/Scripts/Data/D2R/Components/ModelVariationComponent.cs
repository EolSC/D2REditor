using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Model variatiom component. Used for weighted random models in ModelVariationDefinitionComponent
    * 
    */
    public class ModelVariationComponent : ISerializable
    {
        public string component_type;
        public string component_name;
        public float weight = 0.0f;
        public JSONObject model_data;

        public void Deserialize(JSONObject json)
        {
            component_type = json["type"];
            component_name = json["name"];
            weight = ISerializable.DeserializeFloat(json["weight"]);
            model_data = json;
        }

        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = component_type;
            result["name"] = component_name;
            result["weight"] = ISerializable.SerializeFloat(weight);

            result["filename"] = model_data["filename"];
            result["visibleLayers"] = model_data["visibleLayers"];
            result["lightMask"] = model_data["lightMask"];
            result["shadowMask"] = model_data["shadowMask"];
            result["ghostShadows"] = model_data["ghostShadows"];
            result["floorModel"] = model_data["floorModel"];
            result["terrainBlendEnableYUpBlend"] = model_data["terrainBlendEnableYUpBlend"];
            result["terrainBlendMode"] = model_data["terrainBlendMode"]; 
            return result;
        }

    }

}