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
        protected bool valid = true;
        protected bool checkValidComponents = true;


        public JsonPreset(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void SetValid(bool valid)
        {
            this.valid = valid;
        }

        public bool IsValid()
        {
            return valid;
        }

        public bool CheckValidComponents()
        {
            return checkValidComponents;
        }
        public virtual void Deserialize(JSONObject json)
        {
            // Mark as valid before deserialization
            SetValid(true);

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

        private void UpdateEntitiesList()
        {
            List<LevelEntity> validEntities = new List<LevelEntity>();
            foreach(var entity in entities)
            {
                if (entity.gameObject != null)
                {
                    validEntities.Add(entity);
                }
                else
                {
                    Debug.Log("Game object for entity " + entity.name + "is no longer valid");
                }
            }
            entities = validEntities;
        }

        public virtual JSONObject Serialize()
        {
            UpdateEntitiesList();

            JSONObject result = new JSONObject();
            result["dependencies"] = dependencies.Serialize();
            result["entities"] = ISerializable.SerializeList(entities);
            result["type"] = type;
            result["name"] = name;

            return result;
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
