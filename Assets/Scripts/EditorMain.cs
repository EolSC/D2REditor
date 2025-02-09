using System;
using System.IO;
using UnityEditor;
using UnityEngine;


/*
 * Static data. Settings and global classes are here
 */
namespace Diablo2Editor
{

}
/* 
 * Entry-point class for this project. Provides Menu items, defines basic functionality of editor
 */

public class EditorMain : MonoBehaviour
{
    public static Diablo2Editor.EditorSettings settings = new Diablo2Editor.EditorSettings();

    private static LevelContentLoader loader = new LevelContentLoader();
    public static ResourceCache cache = new ResourceCache();


    [MenuItem("Diablo Level Editor/Open level...")]
    private static void OpenLevel()
    {
        // Open file dialog
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        // Load level with no tests
        OpenLevel(absolute_path);
    }

    [MenuItem("Diablo Level Editor/Save level...")]
    private static void SaveLevel()
    {
        var name = LevelContentLoader.GetLevelName();

        var path = EditorUtility.SaveFilePanel(
            "Save level as ds1/json pair",
            "",
            name,
            "ds1");

        if (path.Length != 0)
        {
            loader.SaveLevel(path);
        }
    }

    [MenuItem("Diablo Level Editor/Developer/Test loading")]
    private static void TestLoading()
    {
        // Use path from settings without opening Browse dialog
        string path = Settings().developer.testLevel;
        OpenLevel(path, true, true);
    }

    [MenuItem("Diablo Level Editor/Developer/Reload settings")]
    private static void ReloadSettings()
    {
        Settings().Reload();
    }

    [MenuItem("Diablo Level Editor/Developer/UnitTestFolder")]
    private static void UnitTestFolder()
    {
        // Collect all *.ds1 files in unit test dir
        var pathMapper = Settings().paths;
        var developerSettings = Settings().developer;
        developerSettings.isUnitTestMode = true;
        var testDir = pathMapper.GetAbsolutePath(Settings().developer.unitTestFolder);
        string[] files =
        Directory.GetFiles(testDir, "*.ds1", SearchOption.AllDirectories);

        bool testResult = true;
        if (files.Length > 0)
        {
            float step = 1/(float)files.Length;
            float progress = 0.0f;
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                EditorUtility.DisplayProgressBar("Unit test", "Processing level: " + name, progress);
                if (!testResult)
                {
                    break;
                }

                try
                {
                    OpenLevel(file, true, true, false);
                }
                catch (Exception ex)
                {
                    testResult = false;
                    Debug.Log("Loading of level " + file + "failed with error " + ex.Message);
                }
                progress += step;
            }
        }
        developerSettings.isUnitTestMode = false;

        EditorUtility.ClearProgressBar();
        LevelContentLoader.ClearScene();
        if (testResult)
        {
            Debug.Log("Test folder success!");
        }
        else
        {
            Debug.Log("Test folder failed");

        }

    }

    public static Diablo2Editor.EditorSettings Settings()
    {
        return settings;
    }


    private static void OpenLevel(string path, bool instantiate = true, bool test_serialization = false, bool displayProgress = true)
    {
        /*
         * D2R uses hybid level data so we need both ds1 and json preset to load level properly
         */
        string absolute_path = Settings().paths.GetAbsolutePath(path);
        string fileName = Path.GetFileNameWithoutExtension(absolute_path);
        string local_path = Settings().paths.GetLocalPath(absolute_path);
        string pathToJson = Settings().paths.GetPresetForLevel(local_path);
        byte[] dsContent = { };
        string jsonContent = "";

        bool levelExists = File.Exists(absolute_path);
        bool jsonExists = File.Exists(pathToJson);
        if (levelExists)
        {

            dsContent = File.ReadAllBytes(absolute_path);
        }
        else
        {
            Debug.LogWarning("Level not found " + absolute_path);
        }

        if (jsonExists)
        {

            jsonContent = File.ReadAllText(pathToJson);
        }
        else
        {
            Debug.LogWarning("Preset for level " + absolute_path + " is not found in path " + pathToJson);
        }

        if (levelExists && jsonExists)
        {
            // Load preset
            loader.LoadLevel(fileName, dsContent, jsonContent, instantiate, displayProgress);
            //Perform some tests if they are needed
            if (test_serialization)
            {
                loader.TestLevelLoading(dsContent, jsonContent);
            }
        }
    }
}
