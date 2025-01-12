using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using JetBrains.Annotations;
using SimpleJSON;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;

namespace Diablo2Editor
{

    public abstract class ISerializable
    {
        public abstract JSONObject Serialize();
        public abstract void Deserialize(JSONObject json);

        protected List<T> DeserializeList<T>(JSONObject json, string depType) where T : ISerializable, new()
        {
            List<T> result = new List<T>();
            foreach (JSONObject item in json[depType])
            {
                T serializable = new T();
                serializable.Deserialize(item);
                result.Add(serializable);
            }
            return result;
        }

        protected JSONArray SerializeList<T>(List<T> list) where T : ISerializable
        {
            JSONArray result = new JSONArray();
            foreach(T item in list)
            {
                JSONObject obj = item.Serialize();
                result.Add(obj);
            }
            return result;
        }

        protected float DeserializeFloat(JSONNode json)
        {
            return json.AsFloat;
        }

        protected float SerializeFloat(float value, string format = "{0:f}")
        {
            return value;
        }

    }
    public class LevelPresetDependency : ISerializable
    {
        public string path;

        public override void Deserialize(JSONObject json)
        {
            path = json["path"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["path"] = path;
            return result;
        }
    }

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

    public class LevelSpeicialTiles : ISerializable
    {
        public override void Deserialize(JSONObject obj)
        {
        }


        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            return result;
        }
    }

    public class LevelEntityComponent : ISerializable
    {
        public string type;
        public string name;

        public override void Deserialize(JSONObject json)
        {
            type = json["type"];
            name = json["name"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["type"] = type;
            result["name"] = name;
            return result;
        }

    }

    public class LevelEntityUnknown : LevelEntityComponent
    {

    }
    public class TerrainDefinitionComponent : LevelEntityComponent
    {

    }

    public class TerrainDecalDefinitionComponent : LevelEntityComponent
    {
        public string albedo;
        public string normal;
        public string orm;
        public int stomp;
        public int snapOptions;
        public float parallaxScale;
        public string biomeName;
        public int layerIndex;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            albedo = json["albedo"];
            normal = json["normal"];
            orm = json["orm"];
            stomp = json["stomp"];
            snapOptions = json["snapOptions"];
            parallaxScale = DeserializeFloat(json["parallaxScale"]);
            biomeName = json["biomeName"];
            layerIndex = json["layerIndex"];

        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["albedo"] = albedo;
            result["normal"] = normal;
            result["orm"] = orm;
            result["stomp"] = stomp;
            result["snapOptions"] = snapOptions;
            result["parallaxScale"] = SerializeFloat(parallaxScale);
            result["biomeName"] = biomeName;
            result["layerIndex"] = layerIndex;

            return result;
        }
    }

    public class TerrainStampDefinitionComponent : LevelEntityComponent
    {
        public string mask;
        public float stomp;
        public float stompG;
        public float stompB;
        public int snapOptions;
        public string biomeName;
        public int layerIndexR;
        public int layerIndexG;
        public int layerIndexB;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            mask = json["mask"];

            stomp = DeserializeFloat(json["stomp"]);
            stompG = DeserializeFloat(json["stompG"]);
            stompB = DeserializeFloat(json["stompB"]);

            snapOptions = json["snapOptions"];
            biomeName = json["biomeName"];

            layerIndexR = json["layerIndexR"];
            layerIndexG = json["layerIndexG"];
            layerIndexB = json["layerIndexB"];

        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["mask"] = mask;

            result["stomp"] = SerializeFloat(stomp); 
            result["stompG"] = SerializeFloat(stompG); 
            result["stompB"] = SerializeFloat(stompB); 

            result["snapOptions"] = snapOptions;
            result["biomeName"] = biomeName;

            result["layerIndexR"] = layerIndexR;
            result["layerIndexG"] = layerIndexG;
            result["layerIndexB"] = layerIndexB;

            return result;
        }
    }

    public class PointLightDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Color color = UnityEngine.Color.white;
        public float power = 0.0f;
        public float radius = 0.0f;
        public float attenuation = 0.0f;
        public int lightMask = 0;
        public bool isLocalLight = false;
        public float diffuseContribution = 1.0f;
        public float specularContribution = 0.0f;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject color_obj = json["color"].AsObject;
            color.r = color_obj["x"];
            color.g = color_obj["y"];
            color.b = color_obj["z"];

            power = DeserializeFloat(json["power"]);
            radius = DeserializeFloat(json["radius"]);
            attenuation = DeserializeFloat(json["attenuation"]);

            lightMask = json["lightMask"];
            isLocalLight = json["isLocalLight"];

            diffuseContribution = DeserializeFloat(json["diffuseContribution"]);
            specularContribution = DeserializeFloat(json["specularContribution"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            JSONObject color_obj = new JSONObject();
            color_obj["x"] = color.r;
            color_obj["y"] = color.g;
            color_obj["z"] = color.b;
            result["color"] = color_obj;

            result["power"] = SerializeFloat(power);
            result["radius"] = SerializeFloat(radius);
            result["attenuation"] = SerializeFloat(attenuation);

            result["lightMask"] = lightMask;
            result["isLocalLight"] = isLocalLight;

            result["diffuseContribution"] = SerializeFloat(diffuseContribution);
            result["specularContribution"] = SerializeFloat(specularContribution);

            return result;
        }
    }

    public class VfxDefinitionComponent : LevelEntityComponent
    {
        public string filename;
        public bool hardKillOnDestroy = false;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            filename = json["filename"];
            hardKillOnDestroy = json["hardKillOnDestroy"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            result["hardKillOnDestroy"] = hardKillOnDestroy;

            return result;
        }

    }
    public class PrefabPlacementDefinitionComponent : LevelEntityComponent
    {
        public string prefab;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            prefab = json["prefab"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["prefab"] = prefab;
            return result;
        }
    }

    public class WallTransparencyComponent : LevelEntityComponent
    {
        public int drawOrder = 0;
        public UnityEngine.Vector2Int wallTileLocalCoord;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            drawOrder = json["drawOrder"];
            JSONObject wall_tile_obj = json["wallTileLocalCoord"].AsObject;
            wallTileLocalCoord.x = wall_tile_obj["x"];
            wallTileLocalCoord.y = wall_tile_obj["y"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["drawOrder"] = drawOrder;

            JSONObject local_coord = new JSONObject();
            local_coord["x"] = wallTileLocalCoord.x;
            local_coord["y"] = wallTileLocalCoord.y;
            result["wallTileLocalCoord"] = local_coord;

            return result;
        }

    }

    public class ModelDefinitionComponent: LevelEntityComponent
    {
        public string filename;
        public int visibleLayers;
        public int lightMask;
        public int shadowMask;
        public bool ghostShadows = false;
        public bool floorModel = false;
        public bool terrainBlendEnableYUpBlend = false;
        public int terrainBlendMode = 0;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            filename = json["filename"];
            visibleLayers = json["visibleLayers"];
            lightMask = json["lightMask"];
            shadowMask = json["shadowMask"];
            ghostShadows = json["ghostShadows"];
            floorModel = json["floorModel"];
            terrainBlendEnableYUpBlend = json["terrainBlendEnableYUpBlend"];
            terrainBlendMode = json["terrainBlendMode"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            result["visibleLayers"] = visibleLayers;
            result["lightMask"] = lightMask;
            result["shadowMask"] = shadowMask;
            result["ghostShadows"] = ghostShadows;
            result["floorModel"] = floorModel;
            result["terrainBlendEnableYUpBlend"] = terrainBlendEnableYUpBlend;
            result["terrainBlendMode"] = terrainBlendMode;
            return result;
        }
    }
    public class ModelVariationComponent : LevelEntityComponent
    {
        public float weight = 0.0f;
        public ModelDefinitionComponent model = new ModelDefinitionComponent();

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            weight = DeserializeFloat(json["weight"]);
            model.Deserialize(json);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["weight"] = SerializeFloat(weight);
            result["filename"] = model.filename;
            result["visibleLayers"] = model.visibleLayers;
            result["lightMask"] = model.lightMask;
            result["shadowMask"] = model.shadowMask;
            result["ghostShadows"] = model.ghostShadows;
            result["floorModel"] = model.floorModel;
            result["terrainBlendEnableYUpBlend"] = model.terrainBlendEnableYUpBlend;
            result["terrainBlendMode"] = model.terrainBlendMode;
            return result;
        }

    }

    public class ModelVariationDefinitionComponent : LevelEntityComponent
    {
        public List<ModelVariationComponent> variations = new List<ModelVariationComponent>();
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            variations = DeserializeList<ModelVariationComponent>(json, "variations");
        }

        public override JSONObject Serialize()
        {
            // TODO - весериализовать базу, вес, моедль именно в таком порядке
            JSONObject result = base.Serialize();
            result["variations"] = SerializeList(variations);
            return result;
        }

    }

    public class PhysicsShapeType : LevelEntityComponent
    {

    }

    public class PhysicsFileDefinition : PhysicsShapeType
    {
        public string filename;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            filename = obj["filename"];
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            return result;
        }
    }


    public class PhysicsShapeBox : PhysicsShapeType
    {
        public UnityEngine.Vector3 center;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 extents;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject center = obj["center"].AsObject;
            this.center.x = DeserializeFloat(center["x"]);
            this.center.y = DeserializeFloat(center["y"]);
            this.center.z = DeserializeFloat(center["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = DeserializeFloat(orientation["x"]);
            this.orientation.y = DeserializeFloat(orientation["y"]);
            this.orientation.z = DeserializeFloat(orientation["z"]);
            this.orientation.w = DeserializeFloat(orientation["w"]);

            JSONObject extents = obj["extents"].AsObject;
            this.extents.x = DeserializeFloat(extents["x"]);
            this.extents.y = DeserializeFloat(extents["y"]);
            this.extents.z = DeserializeFloat(extents["z"]);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject center_object = new JSONObject();
            center_object["x"] = SerializeFloat(this.center.x);
            center_object["y"] = SerializeFloat(this.center.y);
            center_object["z"] = SerializeFloat(this.center.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = SerializeFloat(this.orientation.x);
            orientation_object["y"] = SerializeFloat(this.orientation.y);
            orientation_object["z"] = SerializeFloat(this.orientation.z);
            orientation_object["w"] = SerializeFloat(this.orientation.w);

            JSONObject extents_object = new JSONObject();
            extents_object["x"] = SerializeFloat(this.extents.x);
            extents_object["y"] = SerializeFloat(this.extents.y);
            extents_object["z"] = SerializeFloat(this.extents.z);

            result["center"] = center_object;
            result["orientation"] = orientation_object;
            result["extents"] = extents_object;

            return result;
        }
    }

    public class PhysicsFixture : LevelEntityComponent
    {
        public PhysicsShapeType shapeType = null;
        public float friction = 0.0f;
        public float restitution = 0.0f;
        public float rollingresistance = 0.0f;
        public float impulsefactor = 0.0f;
        public float explosionfactor = 0.0f;
        public float explosionliftfactor = 0.0f;
        public float windfactor = 0.0f;
        public float dragfactor = 0.0f;
        public float liftfactor = 0.0f;
        public float density = 0.0f;
        public int boneindex = -1;


        private PhysicsShapeType LoadShape(JSONObject json)
        {
            string type = json["type"];
            if (type == "PhysicsBoxDefinition")
            {
                PhysicsShapeType box = new PhysicsShapeBox();
                box.Deserialize(json);
                return box;
            }
            if (type == "PhysicsFileDefinition")
            {
                PhysicsFileDefinition file = new PhysicsFileDefinition();
                file.Deserialize(json);
                return file;
            }
            return new PhysicsShapeType();

        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            JSONObject shape_object = json["shapetype"].AsObject;

            shapeType = LoadShape(shape_object);
            friction = DeserializeFloat(json["friction"]);
            restitution = DeserializeFloat(json["restitution"]);
            rollingresistance = DeserializeFloat(json["rollingresistance"]);
            impulsefactor = DeserializeFloat(json["impulsefactor"]);
            explosionfactor = DeserializeFloat(json["explosionfactor"]);
            explosionliftfactor = DeserializeFloat(json["explosionliftfactor"]);
            windfactor = DeserializeFloat(json["windfactor"]);
            dragfactor = DeserializeFloat(json["dragfactor"]);
            liftfactor = DeserializeFloat(json["liftfactor"]);
            density = DeserializeFloat(json["density"]);
            boneindex = json["boneindex"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            result["shapetype"] = shapeType.Serialize();
            result["friction"] = SerializeFloat(friction);
            result["restitution"] = SerializeFloat(restitution);
            result["rollingresistance"] = SerializeFloat(rollingresistance);
            result["impulsefactor"] = SerializeFloat(impulsefactor);
            result["explosionfactor"] = SerializeFloat(explosionfactor);
            result["explosionliftfactor"] = SerializeFloat(explosionliftfactor);
            result["windfactor"] = SerializeFloat(windfactor);
            result["dragfactor"] = SerializeFloat(dragfactor);
            result["liftfactor"] = SerializeFloat(liftfactor);
            result["density"] = SerializeFloat(density);
            result["boneindex"] = boneindex;
            return result;
        }
    }

    public class PhysicsBodyDefinitionComponent : LevelEntityComponent
    {
        public string bodytype;
        public List<PhysicsFixture> fixturedefs;
        public string filter;
        public bool allowTransition = true;
        public bool removeOnDeath = true;
        public float lineardamping = 0;
        public float angulardamping = 0;
        public float gravityscale = 0;




        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            bodytype = json["bodytype"];
            fixturedefs = DeserializeList<PhysicsFixture>(json, "fixturedefs");
            filter = json["filter"];
            allowTransition = json["allowTransition"];
            removeOnDeath = json["removeOnDeath"];

            lineardamping = DeserializeFloat(json["lineardamping"]);
            angulardamping = DeserializeFloat(json["angulardamping"]);
            gravityscale = DeserializeFloat(json["gravityscale"]);
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["bodytype"] = bodytype;
            result["fixturedefs"] = SerializeList(fixturedefs);

            result["filter"] = filter;
            result["allowTransition"] = allowTransition;
            result["removeOnDeath"] = removeOnDeath;

            result["lineardamping"] = SerializeFloat(lineardamping);
            result["angulardamping"] = SerializeFloat(angulardamping);
            result["gravityscale"] = SerializeFloat(gravityscale);
            return result;
        }
    }



    public class TransformDefinitionComponent : LevelEntityComponent
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion orientation;
        public UnityEngine.Vector3 scale;
        public bool inheritOnlyPosition = true;

        public override void Deserialize(JSONObject obj)
        {
            base.Deserialize(obj);
            JSONObject position = obj["position"].AsObject;
            this.position.x = DeserializeFloat(position["x"]);
            this.position.y = DeserializeFloat(position["y"]);
            this.position.z = DeserializeFloat(position["z"]);

            JSONObject orientation = obj["orientation"].AsObject;
            this.orientation.x = DeserializeFloat(orientation["x"]);
            this.orientation.y = DeserializeFloat(orientation["y"]);
            this.orientation.z = DeserializeFloat(orientation["z"]);
            this.orientation.w = DeserializeFloat(orientation["w"]);

            JSONObject scale = obj["scale"].AsObject;
            this.scale.x = DeserializeFloat(scale["x"]);
            this.scale.y = DeserializeFloat(scale["y"]);
            this.scale.z = DeserializeFloat(scale["z"]);

            this.inheritOnlyPosition = obj["inheritOnlyPosition"].AsBool;
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            JSONObject pos_object = new JSONObject();
            pos_object["x"] = SerializeFloat(this.position.x);
            pos_object["y"] = SerializeFloat(this.position.y);
            pos_object["z"] = SerializeFloat(this.position.z);

            JSONObject orientation_object = new JSONObject();
            orientation_object["x"] = SerializeFloat(this.orientation.x);
            orientation_object["y"] = SerializeFloat(this.orientation.y);
            orientation_object["z"] = SerializeFloat(this.orientation.z);
            orientation_object["w"] = SerializeFloat(this.orientation.w);

            JSONObject scale_object = new JSONObject();
            scale_object["x"] = SerializeFloat(this.scale.x);
            scale_object["y"] = SerializeFloat(this.scale.y);
            scale_object["z"] = SerializeFloat(this.scale.z);

            result["position"] = pos_object;
            result["orientation"] = orientation_object;
            result["scale"] = scale_object;
            result["inheritOnlyPosition"] = this.inheritOnlyPosition;

            return result;
        }
    }


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
            return new LevelEntityUnknown();
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

    public class TileBiomeOverrides : ISerializable
    {
        public override void Deserialize(JSONObject json)
        {

        }


        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            return result;
        }
    }

    public class SpecialTile : ISerializable
    {
        public override void Deserialize(JSONObject json)
        {

        }


        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            return result;
        }
    }


    public class LevelPreset : ISerializable
    {
        
        public string type;
        public string name;
        public LevelPresetDependencies dependencies = new LevelPresetDependencies();
        public List<LevelEntity> entities = new List<LevelEntity>();
        public LevelEntity terrain = new LevelEntity();
        public string biomeFilename;
        public List<TileBiomeOverrides> perTileBiomeOverrides = new List<TileBiomeOverrides>();
        SpecialTile specialTiles = new SpecialTile();

        public override void Deserialize(JSONObject json)
        {
            dependencies.Deserialize(json["dependencies"].AsObject);
            type = json["type"];
            name = json["name"];
            entities = DeserializeList<LevelEntity>(json, "entities");
            terrain.Deserialize(json["terrain"].AsObject);
            biomeFilename = json["biomeFilename"];
            perTileBiomeOverrides = DeserializeList<TileBiomeOverrides>(json, "perTileBiomeOverrides");
            specialTiles.Deserialize(json["specialTiles"].AsObject);

        }
    

        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            result["dependencies"] = this.dependencies.Serialize();
            result["type"] = type;
            result["name"] = name;
            result["entities"] = SerializeList(entities);
            result["terrain"] = terrain.Serialize();
            result["biomeFilename"] = biomeFilename;
            result["perTileBiomeOverrides"] = SerializeList(perTileBiomeOverrides);
            result["specialTiles"] = specialTiles.Serialize();

            return result;
        }
    }

}
