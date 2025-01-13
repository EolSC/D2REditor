using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{

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





        public override void Deserialize(JSONObject obj)
        {
            particles = DeserializeList<LevelPresetDependency>(obj, "particles");
            models = DeserializeList<LevelPresetDependency>(obj, "models");
            skeletons = DeserializeList<LevelPresetDependency>(obj, "skeletons");
            animations = DeserializeList<LevelPresetDependency>(obj, "animations");
            textures = DeserializeList<LevelPresetDependency>(obj, "textures");
            physics = DeserializeList<LevelPresetDependency>(obj, "physics");
            json = DeserializeList<LevelPresetDependency>(obj, "json");
            variantdata = DeserializeList<LevelPresetDependency>(obj, "variantdata");
            objecteffects = DeserializeList<LevelPresetDependency>(obj, "objecteffects");
            other = DeserializeList<LevelPresetDependency>(obj, "other");
        }


        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["particles"] = SerializeList(particles);
            result["models"] = SerializeList(models);
            result["skeletons"] = SerializeList(skeletons);
            result["animations"] = SerializeList(animations);
            result["textures"] = SerializeList(textures);
            result["physics"] = SerializeList(physics);
            result["json"] = SerializeList(json);
            result["variantdata"] = SerializeList(variantdata);
            result["objecteffects"] = SerializeList(objecteffects);
            result["other"] = SerializeList(other);
            return result;
        }
    }
}
