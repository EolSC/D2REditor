using SimpleJSON;
using System.Collections.Generic;
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
        private const string DATA_ROOT_SETTINGS = "Settings/DataRootSettings.json";
        private const string SETTINGS_FILE = "Settings/D2REditorSettings.json";
        private const string EXAMPLE_FILE = "Settings/DataRootSettingsExample.json";
        public class CommonSettings
        {
            public TextureLoadMode textureLoadMode = TextureLoadMode.All;
            public int modelLodLevel = 0;
            public void Init(JSONNode node)
            {
                JSONObject obj = node as JSONObject;
                modelLodLevel = obj["modelLodLevel"];
                if (obj != null && obj.IsObject)
                {
                    object parsedMode;
                    if (System.Enum.TryParse(typeof(TextureLoadMode),
                        obj["textureLoadMode"], out parsedMode))
                    {
                        textureLoadMode = (TextureLoadMode)parsedMode;
                    }
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
                if (obj != null && obj.IsObject)
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
            // Unit-testing mode. Affects texture loading
            public bool isUnitTestMode = false;
            // Folder for testing loading/saving
            public List<string> unitTestFolders = new List<string>();
            public void Init(JSONNode obj)
            {
                if (obj != null && obj.IsObject)
                {
                    testLevel = obj["testLevel"];
                    unitTestFolders.Clear();
                    JSONArray folders_obj = obj["unitTestFolders"].AsArray;
                    foreach(var path in folders_obj.Values)
                    {
                        unitTestFolders.Add(path);
                    }
                    
                }
            }
        }

        public PathMapper paths = new PathMapper();
        public CommonSettings common = new CommonSettings();
        public DeveloperSettings developer = new DeveloperSettings();
        public CameraSettings camera = new CameraSettings();
        public MapList mapList = new MapList();
        public D2Palette pallete = new D2Palette();
        public LevelTypesLoader levelTypesLoader = new LevelTypesLoader();

        public EditorSettings()
        {
            Reload();
        }

        public void Reload()
        {
            string root_settings_path = Path.Combine(Application.dataPath, DATA_ROOT_SETTINGS);
            if (File.Exists(root_settings_path))
            {
                var jsonContent = File.ReadAllText(root_settings_path);
                JSONNode rootSettings = JSONNode.Parse(jsonContent);
                paths.InitRootFolders(rootSettings["rootFolders"]);
            }
            else
            {
                Debug.Log("Settings file not found. Please copy it from  " + EXAMPLE_FILE + " and set main_folder as path to unpacked D2R data");
            }
            string settings_full_path = Path.Combine(Application.dataPath, SETTINGS_FILE);
            if (File.Exists(settings_full_path))
            {
                var jsonContent = File.ReadAllText(settings_full_path);
                JSONNode settings = JSONNode.Parse(jsonContent);
                paths.Init(settings["paths"]);
                common.Init(settings["common"]);
                developer.Init(settings["developer"]);
                camera.Init(settings["camera"]);
                mapList.InitFromFile(paths.GetPathToMapList(), paths.GetPathToLevelPresets());
            }
            else
            {
                Debug.Log("Settings file not found at path " + SETTINGS_FILE);
            }
            var paletteDir = paths.GetPalettesRoot();
            pallete.Load(paletteDir, paths);
            var levelTypesPath = paths.GetPathToLevelTypes();
            levelTypesLoader.Init(levelTypesPath);

            Debug.Log("Settings loaded: " + SETTINGS_FILE);
        }
    }
}
