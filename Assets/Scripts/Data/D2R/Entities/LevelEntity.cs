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
        public long id;
        public List<LevelEntityComponent> components;
        public JsonPreset preset;
        public GameObject gameObject;

        public LevelEntity(JsonPreset parent) {
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
                bool valid = true;
                var obj = ComponentFactory.CreateComponentByType(component_type, gameObject, ref valid);
                if (preset.CheckValidComponents() && !valid)
                {
                    Debug.LogError("Unknown component type: " + component_type);
                    preset.SetValid(false);
                }
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
