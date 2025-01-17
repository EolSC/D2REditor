using Diablo2Editor;
using LSLib.Granny.GR2;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
namespace Diablo2Editor
{

    public class MeshData
    {
        public UnityEngine.Mesh mesh;
        public List<UnityEngine.Material> materials;
        public Vector3 localScale;
    }

    public class DependencyModel : LevelPresetDependency
    {
        // Hardcoded LOD
        // TODO: transfer it to editor settings, support different LODS
        public static string lod_level = "_lod1.model";

        public override void LoadResource()
        {
            // Find absolute path to model file
            string full_path = PathMapper.GetAbsolutePath(path);
            // Apply lod level
            full_path = full_path.Replace(".model", lod_level);
            if (File.Exists(full_path))
            {
                // Create GR2 model
                var root = LSLib.Granny.GR2Utils.LoadModel(full_path);
                // Instantiate model to Unity
                Load(this.dependencies, root, true);
            }
        }

        private void LoadTexture(LevelPresetDependencies dependencies, string txt, UnityEngine.Material material, string type = "_MainTex")
        {
            string path = PathMapper.GetAbsolutePath(txt);
            UnityEngine.Object resource = dependencies.GetResource(txt, DependencyType.Texture);
            if (resource != null)
            {
                Texture2D tex = resource as Texture2D;
                material.SetTexture(type, tex);
            }
        }

        public void Load(LevelPresetDependencies dependencies, LSLib.Granny.Model.Root root, bool loadTextures)
        {
            List<MeshData> meshes = new List<MeshData>();

            foreach (var m in root.Meshes)
            {
                MeshData meshData = new MeshData();
                var mesh = new UnityEngine.Mesh();
                mesh.name = m.Name;
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
                foreach (var group in groups)
                {
                    UnityEngine.Rendering.SubMeshDescriptor descriptor = new UnityEngine.Rendering.SubMeshDescriptor();
                    descriptor.indexStart = group.TriFirst * 3;
                    descriptor.indexCount = group.TriCount * 3;
                    descriptor.baseVertex = 0;
                    descriptor.topology = MeshTopology.Triangles;
                    submeshes.Add(descriptor);

                }
                mesh.SetSubMeshes(submeshes);

                meshData.localScale = new Vector3(m.ExtendedData.VertexScale, m.ExtendedData.VertexScale, m.ExtendedData.VertexScale);


                // Load materials textures
                if (loadTextures == true)
                {
                    foreach (var binding in m.MaterialBindings)
                    {
                        var material = new UnityEngine.Material(Shader.Find("Standard"));
                        material.EnableKeyword("_NORMALMAP");
                        material.EnableKeyword("_METALLICGLOSSMAP");

                        var albedo = binding.Material?.Maps?.FirstOrDefault(map => map.Usage == "AlbedoTexture");
                        if (albedo != null) LoadTexture(dependencies, albedo.Map.Texture.FromFileName, material);


                        var normal = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "NormalTexture");
                        if (normal != null) LoadTexture(dependencies, normal.Map.Texture.FromFileName, material, "_BumpMap");
                        var orm = m.MaterialBindings[0].Material?.Maps?.FirstOrDefault(map => map.Usage == "ORMTexture");
                        if (orm != null) LoadTexture(dependencies, orm.Map.Texture.FromFileName, material, "_MetallicGlossMap");
                        meshData.materials.Add(material);

                    }
                }
                meshData.mesh = mesh;
                meshes.Add(meshData);


            }
            resource = meshes;
        }



    }
}
