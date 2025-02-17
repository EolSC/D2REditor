using SimpleJSON;
using UnityEngine;
namespace Diablo2Editor
{
    public class VisualDataBoxDefinitionComponent : LevelEntityComponent
    {
        bool enableGlobalLights = false;
        bool enableGradingLutFile = false;
        bool enableHopeLightShadows = false;
        bool enableIBL = false;
        bool enableInsideLighting = false;
        string filename;
        Vector3 min;
        Vector3 max;
        public int priority = 0;
        public string targetId;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            enableGlobalLights = json["enableGlobalLights"];
            enableGradingLutFile = json["enableGradingLutFile"];
            enableHopeLightShadows = json["enableHopeLightShadows"];
            enableIBL = json["enableIBL"];
            enableInsideLighting = json["enableInsideLighting"];

            filename = json["filename"];
            min = ISerializable.DeserializeVector(json["min"].AsObject);
            max = ISerializable.DeserializeVector(json["max"].AsObject);
            priority = json["priority"];
            targetId = json["targetId"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["enableGlobalLights"] = enableGlobalLights;
            result["enableGradingLutFile"] = enableGradingLutFile;
            result["enableHopeLightShadows"] = enableHopeLightShadows;
            result["enableIBL"] = enableIBL;
            result["enableInsideLighting"] = enableInsideLighting;

            result["filename"] = filename;

            result["min"] = ISerializable.SerializeVector(min);
            result["max"] = ISerializable.SerializeVector(max);

            result["priority"] = priority;
            result["targetId"] = targetId;

            return result;
        }

    }
}
