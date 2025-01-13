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

        public override void Deserialize(JSONObject json)
        {
            type = json["type"];
            name = json["name"];
            id = json["id"];

            JSONNode components = json["components"];
            this.components = new List<LevelEntityComponent>();
            foreach (JSONNode component in components)
            {
                var component_type = component["type"];
                var obj = CreateComponentByType(component_type);
                obj.Deserialize(component.AsObject);
                this.components.Add(obj);
            }
        }

        private LevelEntityComponent CreateComponentByType(string type)
        {
            if (type == "TransformDefinitionComponent")
            {
                return new TransformDefinitionComponent();
            }
            if (type == "TerrainDefinitionComponent")
            {
                return new TerrainDefinitionComponent();
            }
            if (type == "ModelDefinitionComponent")
            {
                return new ModelDefinitionComponent();
            }
            if (type == "ModelVariationDefinitionComponent")
            {
                return new ModelVariationDefinitionComponent();
            }
            if (type == "PhysicsBodyDefinitionComponent")
            {
                return new PhysicsBodyDefinitionComponent();
            }
            if (type == "WallTransparencyComponent")
            {
                return new WallTransparencyComponent();
            }
            if (type == "PrefabPlacementDefinitionComponent")
            {
                return new PrefabPlacementDefinitionComponent();
            }
            if (type == "VfxDefinitionComponent")
            {
                return new VfxDefinitionComponent();
            }
            if (type == "TerrainDecalDefinitionComponent")
            {
                return new TerrainDecalDefinitionComponent();
            }
            if (type == "TerrainStampDefinitionComponent")
            {
                return new TerrainStampDefinitionComponent();
            }
            if (type == "PointLightDefinitionComponent")
            {
                return new PointLightDefinitionComponent();
            }
            Debug.Log("Unknown component: " + type);
            return new LevelComponentUnknown();
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = type;
            result["name"] = name;
            result["id"] = id;
            result["components"] = SerializeList(components);
            return result;
        }
    }
}
