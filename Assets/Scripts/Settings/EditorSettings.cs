using SimpleJSON;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Diablo2Editor
{
    /* Storage for all project settings
     * Includes path to D2R extracted data and other system stuff
     */
    public class EditorSettings
    {
        // Settungs file name
        private const string SETTINGS_FILE = "Settings/D2REditorSettings.json";
        private const string EXAMPLE_FILE = "Settings/D2REditorSettings_example.json";
        public class CommonSettings
        {
            public string textureLoadMode;
            public void Init(JSONNode node)
            {
                JSONObject obj = node as JSONObject;
                if (obj)
                {
                    textureLoadMode = obj["textureLoadMode"];
                }
            }
        }
        public class CameraSettings
        {
            public float nearClip = 0.01f;
            public float farClip = 1000f;
            public bool occlusionCulling = true;
            public float zoom = 2.0f;
            public void Init(JSONNode node)
            {
                JSONObject obj = node as JSONObject;
                if (obj)
                {
                    nearClip = obj["nearClip"];
                    farClip = obj["farClip"];
                    occlusionCulling = obj["occlusionCulling"];
                    zoom = obj["zoom"];
                }
            }
        }

        public class DeveloperSettings
        {
            // Path to test level in data folder to speed up menuing

            public string testLevel;
            public void Init(JSONNode obj)
            {
                if (obj != null && obj.IsObject)
                {
                    testLevel = obj["testLevel"];
                }
            }
        }

        public PathMapper paths = new PathMapper();
        public CommonSettings common = new CommonSettings();
        public DeveloperSettings developer = new DeveloperSettings();
        public CameraSettings camera = new CameraSettings();

        public EditorSettings()
        {
            Reload();
        }

        public void Reload()
        {
            string settings_full_path = Path.Combine(Application.dataPath, SETTINGS_FILE);
            if (File.Exists(settings_full_path))
            {
                var jsonContent = File.ReadAllText(settings_full_path);
                JSONNode settings = JSONNode.Parse(jsonContent);
                paths.Init(settings["paths"].AsObject);
                common.Init(settings["common"]);
                developer.Init(settings["developer"]);
                camera.Init(settings["camera"]);
            }
            else
            {
                Debug.Log("Settings file not found. Please copy it from  " + EXAMPLE_FILE + " and set data_root as path to unpacked D2R data");
            }
            Debug.Log("Settings loaded: " + SETTINGS_FILE);
        }

        public void UpdateCameraSettings(GameObject gameObject)
        {
            SceneView.CameraSettings settings = new SceneView.CameraSettings();
            settings.nearClip = camera.nearClip;
            settings.farClip = camera.farClip;
            settings.occlusionCulling = camera.occlusionCulling;
    
            SceneView sceneView = SceneView.lastActiveSceneView;
            sceneView.orthographic = true;
            sceneView.size = camera.zoom;
            sceneView.cameraSettings = settings;

            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                var bounds = renderers[0].bounds;
                for (var i = 1; i < renderers.Length; ++i)
                    bounds.Encapsulate(renderers[i].bounds);
                var center = bounds.center;
                sceneView.pivot = center;
            }
            sceneView.Repaint();
            LogSceneViewPivot();
        }

        public void LogSceneViewPivot()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            var pivot = sceneView.pivot;
            var position = sceneView.position;
            var size = sceneView.size;
            var distance = sceneView.cameraDistance;
            Debug.Log("Camera pivot is " + pivot);
            Debug.Log("Camera position is " + position);
            Debug.Log("Camera size is " + size);
            Debug.Log("Camera cameraDistance is " + distance);
            sceneView.Repaint();

        }
    }
}
