using UnityEngine;
using SimpleJSON;
using System.Linq;
using System;
using System.IO;
using UnityEditor;
using static LSLib.Granny.GR2.Magic;
using LSLib.Granny.Model;
using Unity.VisualScripting;
using System.Collections.Generic;


namespace Diablo2Editor
{
    /*
    * Model component. Contains all 3d models for D2R with textures and materials 
    * 
    */
    public class ModelDefinitionComponent : LevelEntityComponent
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

        public override void Instantiate(LevelPresetDependencies dependencies)
        {
            base.Instantiate(dependencies);
            System.Object obj = dependencies.GetResource(filename, DependencyType.Model);
            if (obj != null)
            {

                List<MeshData> models = obj as List<MeshData>;
                foreach(var model in models)
                {
                    var meshObj = new GameObject(model.mesh.name);
                    var meshFilter = meshObj.AddComponent<MeshFilter>();
                    meshFilter.mesh = model.mesh;
                    var meshRenderer = meshObj.AddComponent<MeshRenderer>();
                    meshRenderer.SetSharedMaterials(model.materials);



                    meshObj.transform.parent = transform;
                    meshObj.transform.localPosition = Vector3.zero;
                    meshObj.transform.localRotation = Quaternion.identity;
                    meshObj.transform.localScale = model.localScale;
                }
            }
        }

        // Loads textures for mesh materials
    }
}