using Diablo2Editor;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

/*
 * Creates scene hierarchy based on level content
 * 
 */
public class ContentDrawer
{
    public void InstantiateContent(GameObject gameObject, LevelComponent component, bool displayProgress = true)
    {
        var d2RData = component.GetD2RLevel();
        var ds1Data = component.GetDS1Level();


        if (d2RData != null)
        {

            if (displayProgress)
            {
                EditorUtility.DisplayProgressBar("Loading level", "Instantiating preset...", 0.0f);
            }
            
            d2RData.Instantiate();
            if (displayProgress)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        if (ds1Data != null)
        {
            GameObject testDS1 = new GameObject();
            testDS1.name = "DS1Level";
            testDS1.transform.parent = gameObject.transform;
            var grid = CreateTileGrid(testDS1);
            grid.levelComponent = component;
            CreateLevelObjects(testDS1, component.GetDS1Level());
        }

        gameObject.name = component.GetName();
        // Flip hierarchy around X-axis
        // D2 granny models use other coordinate system. To show level in same coordinates
        // as game shows it
        var presetGO = d2RData.gameObject;
        presetGO.transform.localScale = new Vector3(-1, 1, 1);

        UpdateCameraSettings(gameObject);
    }

    private void UpdateCameraSettings(GameObject gameObject)
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

    private TileGrid CreateTileGrid(GameObject gameObject)
    {
        GameObject tileGridObject = new GameObject();
        tileGridObject.name = "TileGrid";
        tileGridObject.transform.parent = gameObject.transform;
        var grid = tileGridObject.AddComponent<TileGrid>();
        return grid;
    }

    private void CreateLevelObjects(GameObject gameObject, DS1Level level)
    {
        ObjectsLoader loader = new ObjectsLoader();
        GameObject objParent = new GameObject();
        objParent.transform.parent = gameObject.transform;
        objParent.name = "Objects";

        var objects = level.objects;
        for (int i =0; i < objects.Count; ++i)
        {
            GameObject objHolder = new GameObject();
            objHolder.transform.parent = objParent.transform;
            var objComponent = objHolder.AddComponent<LevelObjectComponent>();
            objComponent.Init(loader, level, i);
        }

        // Flip hierarchy around X-axis
        // D2 granny models use other coordinate system. To show level in same coordinates
        // as game shows it
        objParent.transform.localScale = new Vector3(-1, 1, 1);
    }

}
