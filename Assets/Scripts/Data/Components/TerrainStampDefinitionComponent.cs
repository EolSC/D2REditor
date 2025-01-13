using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class TerrainStampDefinitionComponent : LevelEntityComponent
    {
        public string mask;
        public float stomp;
        public float stompG;
        public float stompB;
        public int snapOptions;
        public string biomeName;
        public long layerIndexR;
        public long layerIndexG;
        public long layerIndexB;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            mask = json["mask"];

            stomp = DeserializeFloat(json["stomp"]);
            stompG = DeserializeFloat(json["stompG"]);
            stompB = DeserializeFloat(json["stompB"]);

            snapOptions = json["snapOptions"];
            biomeName = json["biomeName"];

            layerIndexR = json["layerIndexR"];
            layerIndexG = json["layerIndexG"];
            layerIndexB = json["layerIndexB"];

        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["mask"] = mask;

            result["stomp"] = SerializeFloat(stomp);
            result["stompG"] = SerializeFloat(stompG);
            result["stompB"] = SerializeFloat(stompB);

            result["snapOptions"] = snapOptions;
            result["biomeName"] = biomeName;

            result["layerIndexR"] = layerIndexR;
            result["layerIndexG"] = layerIndexG;
            result["layerIndexB"] = layerIndexB;

            return result;
        }
    }
}