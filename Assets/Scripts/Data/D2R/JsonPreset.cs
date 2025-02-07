using Diablo2Editor;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo2Editor
{

    public class JsonPreset : ISerializable
    {
        public string type;
        public string name;
        public LevelPresetDependencies dependencies;
        public List<LevelEntity> entities;
        public GameObject gameObject;
        protected bool checkMissingComponents = true;


        public JsonPreset(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public bool NeedCheckMissingComponents()
        {
            return checkMissingComponents;
        }
        public virtual void Deserialize(JSONObject json)
        {
            dependencies = new LevelPresetDependencies();
            dependencies.Deserialize(json["dependencies"].AsObject);
            entities = new List<LevelEntity>();
            foreach (JSONObject item in json["entities"])
            {
                LevelEntity entity = new LevelEntity(this);
                entity.Deserialize(item);
                entities.Add(entity);
            }
            type = json["type"];
            name = json["name"];
        }

        public virtual JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["dependencies"] = dependencies.Serialize();
            result["entities"] = ISerializable.SerializeList(entities);
            result["type"] = type;
            result["name"] = name;

            return result;
        }

        public void LoadResources()
        {
            dependencies.LoadResources();
        }

        public virtual void Instantiate()
        {
            foreach (var entity in entities)
            {
                entity.Instantiate();
            }
        }

    }
}
