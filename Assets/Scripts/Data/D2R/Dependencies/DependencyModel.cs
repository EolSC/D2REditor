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
        // Default model lod to use if required is not found
        // Use 0 as default(suffix _lod0) because it's lowest possible lod
        private static int FALLBACK_LOD = 0;

        private bool FindLod(PathMapper pathMapper, int lod, ref string result_path)
        {
            var lodSuffix = "_lod" + lod + ".model";
            // Apply lod level
            var lod_path = path.Replace(".model", lodSuffix);
            // Find absolute path to model file
            result_path = pathMapper.GetAbsolutePath(lod_path);

            return File.Exists(result_path);
        }

        protected override void LoadResource(LevelLoadingStrategy strategy)
        {
            string abs_path = "";
            var pathMapper = strategy.settings.paths;
            var settingsLod = strategy.settings.common.modelLodLevel;
            if (FindLod(pathMapper, settingsLod, ref abs_path))
            {
                LoadNative(this.dependencies, abs_path);
            }
            else
            {
                if (FindLod(pathMapper, FALLBACK_LOD, ref abs_path))
                {
                    LoadNative(this.dependencies, abs_path);
                }

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
