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
   public class LevelPreset : ISerializable
   {
        public int seed = 0;
        public string type;
        public string name;
        public LevelPresetDependencies dependencies;
        public List<LevelEntity> entities;
        public LevelEntity terrain;
        public string biomeFilename;
        public List<TileBiomeOverrides> perTileBiomeOverrides;
        SpecialTile specialTiles;
        public GameObject gameObject;


        public LevelPreset(GameObject gameObject, JSONObject json, int seed)
        {
            this.seed = seed;
            GameObject obj = new GameObject();
            obj.transform.parent = gameObject.transform;
            obj.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            obj.name = "json";
            this.gameObject = obj;
            Deserialize(json);
        }

        public void Deserialize(JSONObject json)
        {
            dependencies = new LevelPresetDependencies();
            dependencies.Deserialize(json["dependencies"].AsObject);
            type = json["type"];
            name = json["name"];
            entities = new List<LevelEntity>();
            foreach (JSONObject item in json["entities"])
            {
                LevelEntity entity = new LevelEntity(this);
                entity.Deserialize(item);
                entities.Add(entity);
            }
            terrain = new LevelEntity(this);
            terrain.Deserialize(json["terrain"].AsObject);
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = ISerializable.DeserializeList<TileBiomeOverrides>(json, "perTileBiomeOverrides");
            specialTiles = new SpecialTile();
            specialTiles.Deserialize(json["specialTiles"].AsObject);
        }

        public void LoadResources()
        {
            dependencies.LoadResources();
        }

        public void Instantiate()
        {
            foreach (var entity in entities)
            {
                entity.Instantiate();
            }
            terrain.Instantiate();
        }



        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["dependencies"] = this.dependencies.Serialize();
            result["type"] = type;
            result["name"] = name;
            result["entities"] = ISerializable.SerializeList(entities);
            result["terrain"] = terrain.Serialize();
            result["biomeFilename"] = biomeFilename;
            result["perTileBiomeOverrides"] = ISerializable.SerializeList(perTileBiomeOverrides);
            result["specialTiles"] = specialTiles.Serialize();

            return result;
        }
    }
}
