using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

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
}


[Serializable]
public class LevelEntity
{
    public string type;
    public string name;
    public Int64 id;
    public LevelEntityComponent[] components;

}





[Serializable]
public class LevelPreset 
{
    public LevelPresetDependencies dependencies;
    public string type;
    public string name;
    public string biomeFilename;
    public string[] perTileBiomeOverrides;
}
