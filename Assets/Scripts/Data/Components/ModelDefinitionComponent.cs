using UnityEngine;
using SimpleJSON;
using System.Linq;
using System;
using System.IO;
using UnityEditor;
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

        public override void Instantiate()
        {
            base.Instantiate();
            var resourceManager = GetDependencies();
            object obj = resourceManager.GetResource(filename, DependencyType.Models);
            if (obj != null)
            {

                List<MeshData> models = obj as List<MeshData>;
                int counter = 0;
                foreach(var model in models)
                {
                    if (model.mesh != null)
                    {
                        string child_name = this.name + counter;
                        var meshObj = new GameObject(child_name);
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
        }
    }
}