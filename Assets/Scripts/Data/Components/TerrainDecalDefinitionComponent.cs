using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class TerrainDecalDefinitionComponent : LevelEntityComponent
    {
        public string albedo;
        public string normal;
        public string orm;
        public int stomp;
        public int snapOptions;
        public float parallaxScale;
        public string biomeName;
        public long layerIndex;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            albedo = json["albedo"];
            normal = json["normal"];
            orm = json["orm"];
            stomp = json["stomp"];
            snapOptions = json["snapOptions"];
            parallaxScale = DeserializeFloat(json["parallaxScale"]);
            biomeName = json["biomeName"];
            layerIndex = json["layerIndex"];

        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["albedo"] = albedo;
            result["normal"] = normal;
            result["orm"] = orm;
            result["stomp"] = stomp;
            result["snapOptions"] = snapOptions;
            result["parallaxScale"] = SerializeFloat(parallaxScale);
            result["biomeName"] = biomeName;
            result["layerIndex"] = layerIndex;

            return result;
        }
    }
}