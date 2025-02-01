using SimpleJSON;

namespace Diablo2Editor
{
    public class DecalDefinitionComponent : LevelEntityComponent
    {
        string diffuseMap;
        string normalMap;
        string ormMap;
        float edgeFade;
        float heightFade;
        float weight;
        bool applyToAlbedo;
        bool applyToNormal;
        bool applyToOcclusion;
        bool applyToRoughness;
        bool applyToMetalness;
        float thresholdHardness;
        float thresholdValue;
        float emissiveIntensity;
        float emissiveFocus;
        int lightMask;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            diffuseMap = json["diffuseMap"];
            normalMap = json["normalMap"];
            ormMap = json["ormMap"];

            edgeFade = ISerializable.DeserializeFloat(json["edgeFade"]);
            heightFade = ISerializable.DeserializeFloat(json["heightFade"]);
            weight = ISerializable.DeserializeFloat(json["weight"]);

            applyToAlbedo = json["applyToAlbedo"];
            applyToNormal = json["applyToNormal"];
            applyToOcclusion = json["applyToOcclusion"];
            applyToRoughness = json["applyToRoughness"];
            applyToMetalness = json["applyToMetalness"];

            thresholdValue = ISerializable.DeserializeFloat(json["thresholdValue"]);
            thresholdHardness = ISerializable.DeserializeFloat(json["thresholdHardness"]);
            emissiveIntensity = ISerializable.DeserializeFloat(json["emissiveIntensity"]);
            emissiveFocus = ISerializable.DeserializeFloat(json["emissiveFocus"]);

            lightMask = json["lightMask"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["diffuseMap"] = diffuseMap;
            result["normalMap"] = normalMap;
            result["ormMap"] = ormMap;

            result["edgeFade"] = ISerializable.SerializeFloat(edgeFade);
            result["heightFade"] = ISerializable.SerializeFloat(heightFade);
            result["weight"] = ISerializable.SerializeFloat(weight);

            result["applyToAlbedo"] = applyToAlbedo;
            result["applyToNormal"] = applyToNormal;
            result["applyToOcclusion"] = applyToOcclusion;
            result["applyToRoughness"] = applyToRoughness;
            result["applyToMetalness"] = applyToMetalness;

            result["thresholdValue"] = ISerializable.SerializeFloat(thresholdValue);
            result["thresholdHardness"] = ISerializable.SerializeFloat(thresholdHardness);
            result["emissiveIntensity"] = ISerializable.SerializeFloat(emissiveIntensity);
            result["emissiveFocus"] = ISerializable.SerializeFloat(emissiveFocus);

            result["lightMask"] = lightMask;

            return result;
        }
    }
}