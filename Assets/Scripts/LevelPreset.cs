using System;
using System.Collections.Generic;
using SimpleJSON;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

[Serializable]
public class LevelPresetDependency
{
    public string path;
}

[Serializable]
public class LevelPresetDependencies
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
}

[Serializable]
public class LevelSpeicialTiles
{

}

[Serializable]
public class LevelEntityComponent
{
    public string type;
    public string name;

    public virtual void Deserialize(JSONNode json)
    {
        string json_text = json.ToString();
        JsonUtility.FromJsonOverwrite(json_text, this);
    }

}

[Serializable]
public class LevelEntityUnknown: LevelEntityComponent
{

}

public class TransformDefinitionComponent : LevelEntityComponent
{
    public UnityEngine.Vector3 position;
    public UnityEngine.Quaternion orientation;
    public UnityEngine.Vector3 scale;
    public bool inheritOnlyPosition = true;
}



public class LevelEntity
{
    public string type;
    public string name;
    public Int64 id;
    public List<LevelEntityComponent> components;

}





[Serializable]
public class LevelPreset 
{
    public LevelPresetDependencies dependencies;
    public string type;
    public string name;
    public List<LevelEntity> entities;
    public LevelEntity terrain;
    public string biomeFilename;
    public string[] perTileBiomeOverrides;

    private LevelEntityComponent CreateComponentByType(string type)
    {
        if (type == "TransformDefinitionComponent")
        {
            return new TransformDefinitionComponent();
        }
        return new LevelEntityUnknown();
    }

    public void CreateComponents(string jsonContent)
    {
        JSONNode root = JSON.Parse(jsonContent);
        JSONNode json_entities = root["entities"];
        this.entities = new List<LevelEntity>();
        foreach (JSONNode entity in json_entities)
        {
            LevelEntity new_entity = LoadEntity(entity);
            this.entities.Add(new_entity);
        }

        JSONNode json_terrain = root["terrain"];
        this.terrain = LoadEntity(json_terrain);
    }

    private LevelEntity LoadEntity(JSONNode entity_node)
    {
        LevelEntity new_entity = new LevelEntity();
        new_entity.type = entity_node["type"];
        new_entity.name = entity_node["name"];
        new_entity.id = entity_node["id"];

        JSONNode components = entity_node["components"];
        new_entity.components = new List<LevelEntityComponent>();
        foreach (JSONNode component in components)
        {
            var component_type = component["type"];
            var obj = CreateComponentByType(component_type);
            obj.Deserialize(component);
            new_entity.components.Add(obj);
        }
        return new_entity;
    }

}
