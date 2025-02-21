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
        public GameObject gameObject;
        protected bool valid = true;
        protected bool checkValidComponents = true;


        public JsonPreset(GameObject gameObject, LevelLoadingStrategy strategy)
        {
            this.gameObject = gameObject;
            this.dependencies = new LevelPresetDependencies(strategy);
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

            
            dependencies.Deserialize(json["dependencies"].AsObject);
            foreach (JSONObject item in json["entities"])
            {
                var entityGO = new GameObject();

                entityGO.transform.SetParent(this.gameObject.transform);
                LevelEntity entity = entityGO.AddComponent<LevelEntity>();
                entity.SetPreset(this);
                entity.Deserialize(item);
            }
            type = json["type"];
            name = json["name"];
        }

        protected List<LevelEntity> GetEntities()
        {
            List<LevelEntity> result = new List<LevelEntity>();
            foreach(Transform child in gameObject.transform)
            {
                LevelEntity entity = child.gameObject.GetComponent<LevelEntity>();
                if(entity != null)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        public virtual JSONObject Serialize()
        {
            var entities = GetEntities();

            JSONObject result = new JSONObject();
            result["dependencies"] = dependencies.Serialize();
            result["entities"] = ISerializable.SerializeList(entities);
            result["type"] = type;
            result["name"] = name;

            return result;
        }

        public virtual void Instantiate()
        {
            var entities = GetEntities();
            foreach (var entity in entities)
            {
                entity.Instantiate();
            }
        }

    }
}
