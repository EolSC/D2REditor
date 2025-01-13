using System;
using System.Collections.Generic;
using SimpleJSON;

namespace Diablo2Editor
{
   public class LevelPreset : ISerializable
   {
        public string type;
        public string name;
        public LevelPresetDependencies dependencies = new LevelPresetDependencies();
        public List<LevelEntity> entities = new List<LevelEntity>();
        public LevelEntity terrain = new LevelEntity();
        public string biomeFilename;
        public List<TileBiomeOverrides> perTileBiomeOverrides = new List<TileBiomeOverrides>();
        SpecialTile specialTiles = new SpecialTile();
        public override void Deserialize(JSONObject json)
        {
            dependencies.Deserialize(json["dependencies"].AsObject);
            type = json["type"];
            name = json["name"];
            entities = DeserializeList<LevelEntity>(json, "entities");
            terrain.Deserialize(json["terrain"].AsObject);
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = DeserializeList<TileBiomeOverrides>(json, "perTileBiomeOverrides");
            specialTiles.Deserialize(json["specialTiles"].AsObject);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["dependencies"] = this.dependencies.Serialize();
            result["type"] = type;
            result["name"] = name;
            result["entities"] = SerializeList(entities);
            result["terrain"] = terrain.Serialize();
            result["biomeFilename"] = biomeFilename;
            result["perTileBiomeOverrides"] = SerializeList(perTileBiomeOverrides);
            result["specialTiles"] = specialTiles.Serialize();

            return result;
        }
    }
}
