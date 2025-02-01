using Diablo2Editor;
using UnityEditor;
using UnityEngine;

public class ContentDrawer
{
    public static void InstantiateContent(GameObject gameObject, Diablo2Editor.LevelPreset preset, Diablo2Editor.DS1Level ds1Level)
    {
        if (preset != null)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading resources...", 0.0f);
            preset.LoadResources();


            EditorUtility.DisplayProgressBar("Loading level", "Instantiating objects...", 0.8f);
            preset.Instantiate();
            EditorUtility.ClearProgressBar();
        }

        if (ds1Level != null)
        {
            GameObject testDS1 = new GameObject();
            testDS1.transform.parent = gameObject.transform;
            DS1Drawer drawer = new DS1Drawer(ds1Level);
            drawer.Instantiate(testDS1);
        }
        UpdateCameraSettings(gameObject);
    }

    private static void UpdateCameraSettings(GameObject gameObject)
    {
        Diablo2Editor.EditorSettings.CameraSettings camera = EditorMain.Settings().camera;
        SceneView.CameraSettings settings = new SceneView.CameraSettings();
        settings.nearClip = camera.nearClip;
        settings.farClip = camera.farClip;
        settings.occlusionCulling = camera.occlusionCulling;

        SceneView sceneView = SceneView.lastActiveSceneView;
        sceneView.orthographic = false;
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
    }

}
