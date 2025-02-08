using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
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
        public static string lod_level = "_lod0.model";

        protected override void LoadResource()
        {
            PathMapper mapper = EditorMain.Settings().paths;
            // Find absolute path to model file
            string full_path = mapper.GetAbsolutePath(path);
            // Apply lod level
            full_path = full_path.Replace(".model", lod_level);
            if (File.Exists(full_path))
            {
                LoadNative(this.dependencies, full_path);
            }
        }

        public void LoadNative(LevelPresetDependencies dependencies, string full_path)
        {
            // Load natively via GrannyLoader
            try
            {
                List<MeshData> data = NativeMeshLoader.LoadGrannyFile(dependencies, full_path);
                resource = data;
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading model " + full_path + e.Message);
            }
        }
    }
}
