using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Diablo2Editor
{
    /*
     * Base class for all .json level data.
     * Contains all the components from preset. Can be constructed from .json and
     * serialized back to .json
     * 
     */
   public class LevelPreset : JsonPreset
   {
        public int seed = 0;
        public LevelEntity terrain;
        public string biomeFilename;
        public List<TileBiomeOverrides> perTileBiomeOverrides;
        SpecialTile specialTiles;

        public LevelPreset(GameObject gameObject, JSONObject json, int seed)
            : base(gameObject)
        {
            this.seed = seed;
            Deserialize(json);
        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);

            terrain = new LevelEntity(this);
            terrain.Deserialize(json["terrain"].AsObject);
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = ISerializable.DeserializeList<TileBiomeOverrides>(json, "perTileBiomeOverrides");
            specialTiles = new SpecialTile();
            specialTiles.Deserialize(json["specialTiles"].AsObject);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["terrain"] = terrain.Serialize();
            result["biomeFilename"] = biomeFilename;
            result["perTileBiomeOverrides"] = ISerializable.SerializeList(perTileBiomeOverrides);
            result["specialTiles"] = specialTiles.Serialize();

            return result;
        }

        public override void Instantiate()
        {
            base.Instantiate();
            terrain.Instantiate();
        }
    }
}
