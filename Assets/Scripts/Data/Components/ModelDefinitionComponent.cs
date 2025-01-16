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
        // Hardcoded LOD
        // TODO: transfer it to editor settings, support different LODS
        public static string lod_level = "_lod1.model";

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

        public void LoadModel()
        {
            // Find absolute path to model file
            string full_path = PathMapper.GetAbsolutePath(filename);
            // Apply lod level
            full_path = full_path.Replace(".model", lod_level);
            if (File.Exists(full_path))
            {
                // Create GR2 model
                var root = LSLib.Granny.GR2Utils.LoadModel(full_path);
                // Instantiate model to Unity
                Load(root, true);
            }

        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadModel();
        }

        // Loads textures for mesh materials
        private void LoadTexture(string txt, UnityEngine.Material material, string type = "_MainTex")
        {
            string path = PathMapper.GetAbsolutePath(txt);
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var formatVal = BitConverter.ToInt16(bytes, 4);
                var width = BitConverter.ToInt32(bytes, 8);
                var height = BitConverter.ToInt32(bytes, 0xC);
                var mipLevels = BitConverter.ToInt32(bytes, 0x1C);
                var startFirstSection = BitConverter.ToInt32(bytes, 0x28);
                var format = formatVal == 31 ? TextureFormat.RGBA32 : formatVal <= 58 ? TextureFormat.DXT1 : formatVal <= 62 ? TextureFormat.DXT5 : TextureFormat.BC4;
                var tex = new Texture2D(width, height, format, mipLevels, mipLevels > 0);
                tex.LoadRawTextureData(bytes.Skip(0x28 + startFirstSection).ToArray());
                tex.Apply();
                material.SetTexture(type, tex);

            }
            else
            {
                Debug.Log("Texture is not found " + path);
            }
        }

        public void Load(LSLib.Granny.Model.Root root, bool loadTextures)
        {
            foreach (var m in root.Meshes)
            {
                var meshObj = new GameObject(m.Name);
                var meshFilter = meshObj.AddComponent<MeshFilter>();
                var mesh = new UnityEngine.Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                // Flip mesh x and mesh normal x.  Unity scene did not match in-game objects
                mesh.vertices = m.PrimaryVertexData.Vertices.Select(v => new Vector3(-1 * v.Position.X, v.Position.Y, v.Position.Z)).ToArray();
                mesh.normals = m.PrimaryVertexData.Vertices.Select(v => new Vector3(v.Normal.X, v.Normal.Y, v.Normal.Z)).ToArray();
                mesh.uv = m.PrimaryVertexData.Vertices.Select(v => new Vector2(v.TextureCoordinates0.X, v.TextureCoordinates0.Y)).ToArray();
                mesh.triangles = m.PrimaryTopology.Indices.ToArray();
                int[] tris = mesh.triangles;
                // flip normals https://stackoverflow.com/questions/51100346/flipping-3d-gameobjects-in-unity3d/51100522
                for (int i = 0; i < tris.Length / 3; i++)
                {
                    int a = tris[i * 3 + 0];
                    int b = tris[i * 3 + 1];
                    int c = tris[i * 3 + 2];
                    tris[i * 3 + 0] = c;
                    tris[i * 3 + 2] = a;
                }
                mesh.triangles = tris;
                var groups = m.PrimaryTopology.Groups;

                /*
                 * Pass Group data from GR2 model to Unity mesh
                 */
                List<UnityEngine.Rendering.SubMeshDescriptor> submeshes = new List<UnityEngine.Rendering.SubMeshDescriptor>();
                foreach( var group in groups)
                {
                    UnityEngine.Rendering.SubMeshDescriptor descriptor = new UnityEngine.Rendering.SubMeshDescriptor();
                    descriptor.indexStart = group.TriFirst * 3;
                    descriptor.indexCount = group.TriCount * 3;
                    descriptor.baseVertex = 0;
                    descriptor.topology = MeshTopology.Triangles;
                    submeshes.Add(descriptor);

                }
                mesh.SetSubMeshes(submeshes);
                meshFilter.mesh = mesh;

                meshObj.transform.localScale = new Vector3(m.ExtendedData.VertexScale, m.ExtendedData.VertexScale, m.ExtendedData.VertexScale);
                var meshRenderer = meshObj.AddComponent<MeshRenderer>();

                // Load materials textures
                if (loadTextures == true)
                {
                    List<UnityEngine.Material> materials = new List<UnityEngine.Material>();
                    foreach( var binding in m.MaterialBindings)
                    {
                        var material = new UnityEngine.Material(Shader.Find("Standard"));
                        material.EnableKeyword("_NORMALMAP");
                        material.EnableKeyword("_METALLICGLOSSMAP");

                        var albedo = binding.Material?.Maps?.FirstOrDefault(map => map.Usage == "AlbedoTexture");
                        if (albedo != null) LoadTexture(albedo.Map.Texture.FromFileName, material);


                           var normal = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "NormalTexture");
                            if (normal != null) LoadTexture(normal.Map.Texture.FromFileName, material, "_BumpMap");
                            var orm = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "ORMTexture");
                            if (orm != null) LoadTexture(orm.Map.Texture.FromFileName, material, "_MetallicGlossMap");
                        materials.Add(material);

                    }
                    meshRenderer.SetSharedMaterials(materials);

                }

                //Setup transform for child object
                meshObj.transform.parent = transform;
                meshObj.transform.localPosition = Vector3.zero;
                meshObj.transform.localRotation = Quaternion.identity;
            }
        }
    }
}