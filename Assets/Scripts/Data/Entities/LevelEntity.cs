using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;

namespace Diablo2Editor
{
    public class LevelEntity : ISerializable
    {
        public string type;
        public string name;
        public Int64 id;
        public List<LevelEntityComponent> components;
        private GameObject owner;

        public LevelEntity(GameObject gameObject) {
            owner = gameObject;
        }


        public void Deserialize(JSONObject json)
        {
            type = json["type"];
            name = json["name"];
            id = json["id"];

            GameObject childObj = new GameObject();
            childObj.name = name;
            childObj.transform.SetParent(owner.transform);

            JSONNode components = json["components"];
            this.components = new List<LevelEntityComponent>();
            foreach (JSONNode component in components)
            {
                var component_type = component["type"];
                var obj = CreateComponentByType(component_type, childObj);
                obj.Deserialize(component.AsObject);
                obj.OnLoaded();
                this.components.Add(obj);
            }
        }

        private LevelEntityComponent CreateComponentByType(string type, GameObject gameObject)
        {
            if (type == "TransformDefinitionComponent")
            {
                return gameObject.AddComponent<TransformDefinitionComponent>();
            }
            if (type == "TerrainDefinitionComponent")
            {
                return gameObject.AddComponent<TerrainDefinitionComponent>(); 
            }
            if (type == "ModelDefinitionComponent")
            {
                return gameObject.AddComponent<ModelDefinitionComponent>();
            }
            if (type == "ModelVariationDefinitionComponent")
            {
                return gameObject.AddComponent<ModelVariationDefinitionComponent>();
            }
            if (type == "PhysicsBodyDefinitionComponent")
            {
                return gameObject.AddComponent<PhysicsBodyDefinitionComponent>();
            }
            if (type == "WallTransparencyComponent")
            {
                return gameObject.AddComponent<WallTransparencyComponent>();
            }
            if (type == "PrefabPlacementDefinitionComponent")
            {
                return gameObject.AddComponent<PrefabPlacementDefinitionComponent>();
            }
            if (type == "VfxDefinitionComponent")
            {
                return gameObject.AddComponent<VfxDefinitionComponent>();
            }
            if (type == "TerrainDecalDefinitionComponent")
            {
                return gameObject.AddComponent<TerrainDecalDefinitionComponent>();
            }
            if (type == "TerrainStampDefinitionComponent")
            {
                return gameObject.AddComponent<TerrainStampDefinitionComponent>();
            }
            if (type == "PointLightDefinitionComponent")
            {
                return gameObject.AddComponent<PointLightDefinitionComponent>();
            }
            Debug.Log("Unknown component: " + type);
            return gameObject.AddComponent<LevelComponentUnknown>();
        }

        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = type;
            result["name"] = name;
            result["id"] = id;
            result["components"] = ISerializable.SerializeList(components);
            return result;
        }
    }
}
