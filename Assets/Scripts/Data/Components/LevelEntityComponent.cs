using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
     * Base class for any ingame component. 
     * OnLoaded method is called after Deserialization. Any work related to 
     * instancing component in Scene should be done there
     */
    public class LevelEntityComponent : MonoBehaviour, ISerializable
    {
        public string component_type;
        public string component_name;
        public LevelEntity entity;

        public virtual void Deserialize(JSONObject json)
        {
            component_type = json["type"];
            component_name = json["name"];
        }

        public virtual void Instantiate()
        {

        }

        protected LevelPresetDependencies GetDependencies()
        {
            if (entity != null)
            {
                if (entity.preset != null)
                {
                    return entity.preset.dependencies;
                }
                else
                {
                    Debug.LogError("Preset not set");
                }
            }
            else
            {
                Debug.LogError("Entity not set");
            }
            return null;
        }


        protected LevelPreset GetPreset()
        {
            return entity.preset;
        }

        public virtual JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = component_type;
            result["name"] = component_name;
            return result;
        }

    }
}