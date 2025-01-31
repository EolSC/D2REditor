using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;

namespace Diablo2Editor
{
    /*
     * Class for any ingame entity contained in json preset.
     */
    public class LevelEntity : ISerializable
    {
        public string type;
        public string name;
        public Int64 id;
        public List<LevelEntityComponent> components;
        public LevelPreset preset;
        public GameObject gameObject;

        public LevelEntity(LevelPreset parent) {
            this.preset = parent;
        }


        public void Deserialize(JSONObject json)
        {
            type = json["type"];
            name = json["name"];
            id = json["id"];

            gameObject = new GameObject();
            gameObject.name = name;
            gameObject.transform.SetParent(preset.gameObject.transform);

            JSONNode components = json["components"];
            this.components = new List<LevelEntityComponent>();
            foreach (JSONNode component in components)
            {
                var component_type = component["type"];
                var obj = ComponentFactory.CreateComponentByType(component_type, gameObject);
                obj.entity = this;
                obj.Deserialize(component.AsObject);
                this.components.Add(obj);
            }
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

        public void Instantiate()
        {
            foreach (var comp in components)
            {
                comp.Instantiate();
            }
        }
    }
}
