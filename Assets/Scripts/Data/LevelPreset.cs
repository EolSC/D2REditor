using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

namespace Diablo2Editor
{
   public class LevelPreset : ISerializable
   {
        public string type;
        public string name;
        public LevelPresetDependencies dependencies;
        public List<LevelEntity> entities;
        public LevelEntity terrain;
        public string biomeFilename;
        public List<TileBiomeOverrides> perTileBiomeOverrides;
        SpecialTile specialTiles;
        private GameObject owner;


        public LevelPreset(JSONObject json, GameObject owner)
        {
            this.owner = owner;
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
                LevelEntity entity = new LevelEntity(owner);
                entity.Deserialize(item);
                entities.Add(entity);
            }
            terrain = new LevelEntity(owner);
            terrain.Deserialize(json["terrain"].AsObject);
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = ISerializable.DeserializeList<TileBiomeOverrides>(json, "perTileBiomeOverrides");
            specialTiles = new SpecialTile();
            specialTiles.Deserialize(json["specialTiles"].AsObject);
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
