using UnityEngine;
using SimpleJSON;
using System.Linq;
using System;
using System.IO;
using UnityEditor;


namespace Diablo2Editor
{
    public class ModelDefinitionComponent : LevelEntityComponent
    {
        public static string lod_level = "_lod4.model";

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
            string full_path = OpenLevelScript.GetAbsolutePath(filename);
            full_path = full_path.Replace(".model", lod_level);
            if (File.Exists(full_path))
            {
                var root = LSLib.Granny.GR2Utils.LoadModel(full_path);
                Load(root, true);
            }

        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadModel();
        }

        public void LoadTexture(String txt, MeshRenderer mr, String type = "_MainTex")
        {
            var path = "\\" + txt;
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var formatVal = BitConverter.ToInt16(bytes, 4);
                var width = BitConverter.ToInt32(bytes, 8);
                var height = BitConverter.ToInt32(bytes, 0xC);
                var mipLevels = BitConverter.ToInt32(bytes, 0x1C);
                var channels = BitConverter.ToInt32(bytes, 0x20);
                var sizeSection1 = BitConverter.ToInt32(bytes, 0x24);
                var startFirstSection = BitConverter.ToInt32(bytes, 0x28);
                var format = formatVal == 31 ? TextureFormat.RGBA32 : formatVal <= 58 ? TextureFormat.DXT1 : formatVal <= 62 ? TextureFormat.DXT5 : TextureFormat.BC4;

                var tex = new Texture2D(width, height, format, mipLevels, mipLevels > 0);
                tex.LoadRawTextureData(bytes.Skip(0x28 + startFirstSection).ToArray());
                tex.Apply();
                mr.material.SetTexture(type, tex);

            }
        }

        public void Load(LSLib.Granny.Model.Root root, bool loadTextures)
        {
            foreach (var m in root.Meshes)
            {
                var meshObj = new GameObject(m.Name);
                var meshRenderer = meshObj.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

                var meshFilter = meshObj.AddComponent<MeshFilter>();
                var mesh = new Mesh();
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
                    // tris[i * 3 + 1] = b;  // is this needed? b is kept the same?
                    tris[i * 3 + 2] = a;
                }
                mesh.triangles = tris;
                meshFilter.mesh = mesh;

                meshObj.transform.localScale = new Vector3(m.ExtendedData.VertexScale, m.ExtendedData.VertexScale, m.ExtendedData.VertexScale);

                if (loadTextures == true)
                {
                    if (meshRenderer.material == null) meshRenderer.material = new Material(Shader.Find("Standard"));
                    meshRenderer.material.EnableKeyword("_NORMALMAP");
                    meshRenderer.material.EnableKeyword("_METALLICGLOSSMAP");

                    if (m.MaterialBindings.Count > 0)
                    {
                        var albedo = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "AlbedoTexture");
                        if (albedo != null) LoadTexture(albedo.Map.Texture.FromFileName, meshRenderer);
                        var normal = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "NormalTexture");
                        if (normal != null) LoadTexture(normal.Map.Texture.FromFileName, meshRenderer, "_BumpMap");
                        var orm = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "ORMTexture");
                        if (orm != null) LoadTexture(orm.Map.Texture.FromFileName, meshRenderer, "_MetallicGlossMap");
                    }
                }

                meshObj.transform.parent = transform;
            }
        }
    }
}