using SimpleJSON;

namespace Diablo2Editor
{
    public class SpotLightDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Color color = UnityEngine.Color.white;
        public float power = 0.0f;
        public float angle = 0.0f;
        public float distance = 0.0f;
        public float attenuation = 0.0f;
        public bool shadowCaster = false;
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
            angle = ISerializable.DeserializeFloat(json["angle"]);
            distance = ISerializable.DeserializeFloat(json["distance"]);
            attenuation = ISerializable.DeserializeFloat(json["attenuation"]);

            shadowCaster = json["shadowCaster"];
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
            result["angle"] = ISerializable.SerializeFloat(angle);
            result["distance"] = ISerializable.SerializeFloat(distance);
            result["attenuation"] = ISerializable.SerializeFloat(attenuation);

            result["shadowCaster"] = shadowCaster;
            result["lightMask"] = lightMask;
            result["isLocalLight"] = isLocalLight;

            result["diffuseContribution"] = ISerializable.SerializeFloat(diffuseContribution);
            result["specularContribution"] = ISerializable.SerializeFloat(specularContribution);

            return result;
        }
    }
}