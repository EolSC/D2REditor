using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
     * Point light component. Not implemented in Unity
     * 
     */
    public class PointLightDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Color color = UnityEngine.Color.white;
        public float power = 0.0f;
        public float radius = 0.0f;
        public float attenuation = 0.0f;
        public int lightMask = 0;
        public bool isLocalLight = false;
        public float diffuseContribution = 1.0f;
        public float specularContribution = 0.0f;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject color_obj = json["color"].AsObject;
            color.r = color_obj["x"];
            color.g = color_obj["y"];
            color.b = color_obj["z"];

            power = ISerializable.DeserializeFloat(json["power"]);
            radius = ISerializable.DeserializeFloat(json["radius"]);
            attenuation = ISerializable.DeserializeFloat(json["attenuation"]);

            lightMask = json["lightMask"];
            isLocalLight = json["isLocalLight"];

            diffuseContribution = ISerializable.DeserializeFloat(json["diffuseContribution"]);
            specularContribution = ISerializable.DeserializeFloat(json["specularContribution"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            JSONObject color_obj = new JSONObject();
            color_obj["x"] = color.r;
            color_obj["y"] = color.g;
            color_obj["z"] = color.b;
            result["color"] = color_obj;

            result["power"] = ISerializable.SerializeFloat(power);
            result["radius"] = ISerializable.SerializeFloat(radius);
            result["attenuation"] = ISerializable.SerializeFloat(attenuation);

            result["lightMask"] = lightMask;
            result["isLocalLight"] = isLocalLight;

            result["diffuseContribution"] = ISerializable.SerializeFloat(diffuseContribution);
            result["specularContribution"] = ISerializable.SerializeFloat(specularContribution);

            return result;
        }
    }
}