using System.Collections.Generic;
using SimpleJSON;
using UnityEditor.ShaderGraph.Serialization;
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
        public List<TileBiomeOverride> perTileBiomeOverrides;
        SpecialTiles specialTiles;

        public LevelPreset(GameObject gameObject, LevelLoadingStrategy strategy, JSONObject json, int seed)
            : base(gameObject, strategy)
        {
            this.seed = seed;
            Deserialize(json);
        }

        public LevelEntity FindTerrain(JSONObject json)
        {
            long id = json["id"];
            foreach (var entity in this.entities)
            {
                if (entity.id == id)
                {
                    return entity;
                }
            }
            return null;
        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject terrain_obj = json["terrain"].AsObject;
            // Some levels have no terrain, it's not an error
            if (terrain_obj.Count > 0)
            {
                if (entities.Count > 0)
                {
                    // If we do have one - find it
                    terrain = FindTerrain(terrain_obj);
                    if (terrain == null)
                    {
                        Debug.LogError("Terrain component is not found");
                        SetValid(false);
                    }
                }
                else
                {
                    // if we have no entities but do have terrian - load it up
                    terrain = new LevelEntity(this);
                    terrain.Deserialize(terrain_obj);
                }

            }
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = ISerializable.DeserializeList<TileBiomeOverride>(json, "perTileBiomeOverrides");
            specialTiles = new SpecialTiles();
            specialTiles.Deserialize(json["specialTiles"].AsObject);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            if (terrain != null)
            {
                result["terrain"] = terrain.Serialize();
            }
            else
            {
                // if no terrain on level - just fill it with empty object
                result["terrain"] = new JSONObject();
            }
            
            result["biomeFilename"] = biomeFilename;
            result["perTileBiomeOverrides"] = ISerializable.SerializeList(perTileBiomeOverrides);
            result["specialTiles"] = specialTiles.Serialize();

            return result;
        }

        public override void Instantiate()
        {
            base.Instantiate();
            if (terrain != null && entities.Count == 0)
            {
                terrain.Instantiate();
            }
        }
    }
}
