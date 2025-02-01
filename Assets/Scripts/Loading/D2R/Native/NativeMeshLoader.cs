using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Diablo2Editor;
using static UnityEngine.Mesh;

public class NativeGrannyWrapper
{
    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "OpenFile", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* OpenFile(string ptchar);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "CloseFile", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void CloseFile(void* context);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetModelCount", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern int GetModelCount(void* context);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetMeshCount", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern int GetMeshCount(void* model);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetModel", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* GetModel(void* context, int index);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetMesh", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* GetMesh(void* model, int index);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetMeshVertexScale", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern float GetMeshVertexScale(void* mesh);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "CopyMeshVertices", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* CopyMeshVertices(void* mesh, ref int count, ref int vert_size);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "CopyMeshIndices", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* CopyMeshIndices(void* mesh, ref int count, ref int index_size);
    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "DeleteMeshBuffer", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void DeleteMeshBuffer(void* buffer);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetSubmeshesCount", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern int GetSubmeshesCount(void* mesh);
    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetSubmeshInfo", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void GetSubmeshInfo(void* mesh, int index, ref int triStart, ref int triCount);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetMaterialsCount", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern int GetMaterialsCount(void* mesh);
    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetMaterial", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern void* GetMaterial(void* mesh, int index);

    [DllImport(@"GrannyLoader.dll", EntryPoint =
    "GetTextureFile", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public unsafe static extern IntPtr GetTextureFile(void* material, string usage);
}

public class NativeMeshLoader
{
    public enum MapUsage
    {
        AlbedoTexture,
        NormalTexture,
        ORMTexture,
    }

    public unsafe static List<Diablo2Editor.MeshData> LoadGrannyFile(LevelPresetDependencies deps, string full_file_name)
    {
        List<Diablo2Editor.MeshData> result = new List<Diablo2Editor.MeshData>();
        void* context = NativeGrannyWrapper.OpenFile(full_file_name);
        if (context != null)
        {
            int modelCount = NativeGrannyWrapper.GetModelCount(context);
            for (int i = 0; i < modelCount; i++)
            {
                void *modelHandle = NativeGrannyWrapper.GetModel(context, i);
                if (modelHandle != null)
                {
                    int meshCount = NativeGrannyWrapper.GetMeshCount(modelHandle);
                    for (int j = 0; j < meshCount; j++)
                    {
                        void *meshHandle = NativeGrannyWrapper.GetMesh(modelHandle, j);
                        if (meshHandle != null)
                        {
                            Diablo2Editor.MeshData data = LoadMesh(deps, meshHandle);
                            result.Add(data);
                        }
                    }
                }
            }
            NativeGrannyWrapper.CloseFile(context);
        }

        return result;
    }

    public unsafe static void LoadMeshData(void* meshHandle, UnityEngine.Mesh unity_mesh)
    {
        MeshDataArray array = AllocateWritableMeshData(1);
        var data = array[0];
        {
            int vertex_count = 0;
            int vert_size = 0;

            void* vertices = NativeGrannyWrapper.CopyMeshVertices(meshHandle, ref vertex_count, ref vert_size);
            int stride = vert_size / sizeof(float);
            int max = vertex_count * stride;
            long vertex_size_in_bytes = vertex_count * vert_size;


            var layout = new[]
            {
                    new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                    new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                    new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
            };

            data.SetVertexBufferParams(vertex_count, layout);
            NativeArray<float> verts = data.GetVertexData<float>();
            void* mesh_vertices_pointer = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(verts);
            Buffer.MemoryCopy(vertices, mesh_vertices_pointer, vertex_size_in_bytes, vertex_size_in_bytes);
            NativeGrannyWrapper.DeleteMeshBuffer(vertices);
        }

        {
            int index_count = 0;
            int bytes_per_index = 0;
            void* indexes = NativeGrannyWrapper.CopyMeshIndices(meshHandle, ref index_count, ref bytes_per_index);
            long index_size_bytes = index_count * bytes_per_index;

            void* dest_indexes = null;
            IndexFormat indexFormat = (bytes_per_index == 2) ? IndexFormat.UInt16 : IndexFormat.UInt32;
            data.SetIndexBufferParams(index_count, indexFormat);
            NativeArray<ushort> index_array = data.GetIndexData<ushort>();
            dest_indexes = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(index_array);
            Buffer.MemoryCopy(indexes, dest_indexes, index_size_bytes, index_size_bytes);
            NativeGrannyWrapper.DeleteMeshBuffer(indexes);
        }

        ApplyAndDisposeWritableMeshData(array, unity_mesh);
    }
    public unsafe static Diablo2Editor.MeshData LoadMesh(LevelPresetDependencies deps, void *meshHandle)
    {
        Diablo2Editor.MeshData meshData = new Diablo2Editor.MeshData();
        var unity_mesh = new Mesh();
        float vertex_scale = NativeGrannyWrapper.GetMeshVertexScale(meshHandle);
        meshData.localScale = new Vector3(vertex_scale, vertex_scale, vertex_scale);
        meshData.mesh = unity_mesh;

        LoadMeshData(meshHandle, unity_mesh);
        unity_mesh.RecalculateBounds();



        int submeshesCount = NativeGrannyWrapper.GetSubmeshesCount(meshHandle);
        if (submeshesCount > 0)
        {
            List<UnityEngine.Rendering.SubMeshDescriptor> submeshes = new List<UnityEngine.Rendering.SubMeshDescriptor>();
            for (int i = 0; i < submeshesCount; i++)
            {
                int triStart = 0;
                int triCount = 0;
                NativeGrannyWrapper.GetSubmeshInfo(meshHandle, i, ref triStart, ref triCount);
                UnityEngine.Rendering.SubMeshDescriptor descriptor = new UnityEngine.Rendering.SubMeshDescriptor();
                descriptor.indexStart = triStart * 3;
                descriptor.indexCount = triCount * 3;
                descriptor.baseVertex = 0;
                descriptor.topology = MeshTopology.Triangles;
                submeshes.Add(descriptor);
            }
            unity_mesh.SetSubMeshes(submeshes);

        }
            
        int materialsCount = NativeGrannyWrapper.GetMaterialsCount(meshHandle);
        if (materialsCount > 0)
        {
            meshData.materials = new List<Material>();
            for (int i = 0; (i < materialsCount); i++)
            {
                void *materialHandle = NativeGrannyWrapper.GetMaterial(meshHandle, i);
                var material = LoadMaterial(deps, materialHandle);
                meshData.materials.Add(material);
            }
        }
        

        return meshData;
    }

    public unsafe static UnityEngine.Material LoadMaterial(LevelPresetDependencies deps, void* materialHandle)
    {
        var material = new UnityEngine.Material(Shader.Find("Standard"));
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_METALLICGLOSSMAP");
        material.SetFloat("_GlossMapScale", 0.0f);
        foreach (MapUsage usage in System.Enum.GetValues(typeof(MapUsage)))
        {
            string path = LoadMaterialMap(materialHandle, usage.ToString());
            if (path.Length > 0)
            {
                object resource = deps.GetResource(path, DependencyType.Textures);
                if (resource != null)
                {
                    Texture2D tex = resource as Texture2D;
                    string type = FromUsageToUnityMapName(usage);
                    material.SetTexture(type, tex);
                }
            }
        }
        return material;
    }

    public static string FromUsageToUnityMapName(MapUsage usage)
    {
        switch (usage)
        {
            default:
            case MapUsage.AlbedoTexture:
                return "_MainTex";
            case MapUsage.NormalTexture:
                return "_BumpMap";
            case MapUsage.ORMTexture:
                return "_MetallicGlossMap";
        }
    }
    public unsafe static string LoadMaterialMap(void* materialHandle, string usage)
    {
        string path = Marshal.PtrToStringAnsi(NativeGrannyWrapper.GetTextureFile(materialHandle, usage));
        if (path != null && path.Length > 0)
        {
            return path;
        }
        return "";
    }
}
