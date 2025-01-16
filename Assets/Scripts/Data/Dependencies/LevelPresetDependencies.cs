using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
     * Container to hold all level dependencies
     * TODO: make it coherent with edits
     */

    public class LevelPresetDependencies : ISerializable
    {
        public List<LevelPresetDependency> particles;
        public List<LevelPresetDependency> models;
        public List<LevelPresetDependency> skeletons;
        public List<LevelPresetDependency> animations;
        public List<LevelPresetDependency> textures;
        public List<LevelPresetDependency> physics;
        public List<LevelPresetDependency> json;
        public List<LevelPresetDependency> variantdata;
        public List<LevelPresetDependency> objecteffects;
        public List<LevelPresetDependency> other;





        public void Deserialize(JSONObject obj)
        {
            particles = ISerializable.DeserializeList<LevelPresetDependency>(obj, "particles");
            models = ISerializable.DeserializeList<LevelPresetDependency>(obj, "models");
            skeletons = ISerializable.DeserializeList<LevelPresetDependency>(obj, "skeletons");
            animations = ISerializable.DeserializeList<LevelPresetDependency>(obj, "animations");
            textures = ISerializable.DeserializeList<LevelPresetDependency>(obj, "textures");
            physics = ISerializable.DeserializeList<LevelPresetDependency>(obj, "physics");
            json = ISerializable.DeserializeList<LevelPresetDependency>(obj, "json");
            variantdata = ISerializable.DeserializeList<LevelPresetDependency>(obj, "variantdata");
            objecteffects = ISerializable.DeserializeList<LevelPresetDependency>(obj, "objecteffects");
            other = ISerializable.DeserializeList<LevelPresetDependency>(obj, "other");
        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["particles"] = ISerializable.SerializeList(particles);
            result["models"] = ISerializable.SerializeList(models);
            result["skeletons"] = ISerializable.SerializeList(skeletons);
            result["animations"] = ISerializable.SerializeList(animations);
            result["textures"] = ISerializable.SerializeList(textures);
            result["physics"] = ISerializable.SerializeList(physics);
            result["json"] = ISerializable.SerializeList(json);
            result["variantdata"] = ISerializable.SerializeList(variantdata);
            result["objecteffects"] = ISerializable.SerializeList(objecteffects);
            result["other"] = ISerializable.SerializeList(other);
            return result;
        }
    }
}
