using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    public class SpecialTile: ISerializable
    {
        public string name;
        public string presetFilename;
        public int tileIdentifier;
        public string type;

        public void Deserialize(JSONObject json)
        {
            name = json["name"];
            presetFilename = json["presetFilename"];
            tileIdentifier = json["tileIdentifier"];
            type = json["type"];
        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            
            result["name"] = name;
            result["presetFilename"] = presetFilename;
            result["tileIdentifier"] = tileIdentifier;
            result["type"] = type;

            return result;
        }
    }

    public class SpecialTiles : ISerializable
    {
        Dictionary<string, List<SpecialTile>> tiles = new Dictionary<string, List<SpecialTile>>();
        public void Deserialize(JSONObject json)
        {
            foreach (var key  in json.Keys)
            {
                var tileList = ISerializable.DeserializeList<SpecialTile>(json, key);
                tiles.Add(key, tileList);
            }
        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            foreach (var key in tiles.Keys)
            {
                result[key] = ISerializable.SerializeList(tiles[key]);
            }
            return result;
        }
    }
}