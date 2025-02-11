using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;


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
    public static ObjectsLoader objectLoader = new ObjectsLoader();
    public static DT1Cache dt1Cache = new DT1Cache();


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
        var testStartTime = Time.realtimeSinceStartup;
        // Collect all *.ds1 files in unit test dir
        var pathMapper = Settings().paths;
        var developerSettings = Settings().developer;
        developerSettings.isUnitTestMode = true;

        var folders = Settings().developer.unitTestFolders;
        int count = 0;

        foreach (var folder in folders)
        {
            var testDir = pathMapper.GetAbsolutePath(folder);
            string[] folderFiles =
            Directory.GetFiles(testDir, "*.ds1", SearchOption.AllDirectories);
            count += folderFiles.Length;
        }

        bool testResult = true;
        if (count > 0)
        {
            float step = 1/(float)count;
            float progress = 0.0f;

            foreach (var file in folders)
            {
                var testDir = pathMapper.GetAbsolutePath(file);
                // Search directory
                testResult = UnitTestOneFolder(testDir, step, ref progress, SearchOption.TopDirectoryOnly);
                var subdirs = Directory.GetDirectories(testDir);
                // Then subdirectories
                foreach (var subdir in subdirs)
                {
                    // Test 1 folder
                    testResult = UnitTestOneFolder(subdir, step, ref progress, SearchOption.AllDirectories);
                    if (!testResult)
                    {
                        break;
                    }
                }
            }
        }
        developerSettings.isUnitTestMode = false;

        EditorUtility.ClearProgressBar();
        LevelContentLoader.ClearScene();
        dt1Cache.Clear();

        var testEndTime = Time.realtimeSinceStartup;
        var diffTime = testEndTime - testStartTime;

        string resultString = testResult ? "Success. " : "Failure. ";
        string elapesdTime = "Elapsed time is " + diffTime + " seconds ";
        string statsString = "Tested " + count + " levels in " + folders.Count + " folders. Test result is " + resultString + elapesdTime;
        UnityEngine.Debug.Log(statsString);

    }
    
    private static async void CleanUpMemory()
    {
        // Do some cleaning 'cause Unit test can be very memory-demanding

        cache.Clear();                                 // Cache is likely to be ineffective for separate folders so it's good idea to clean it up here
        var task = Resources.UnloadUnusedAssets();     // Unused assets can also have big impact. 
        while (!task.isDone)
        {
            await task;
        }
        System.GC.Collect(0, GCCollectionMode.Forced, true);                            // Run GC to sweep all unused data
    }

    private static bool UnitTestOneFolder(string folder, float step, ref float progress, SearchOption option)
    {
        CleanUpMemory();

        bool testResult = true;
        string[] folderFiles =
        Directory.GetFiles(folder, "*.ds1", option);
        foreach (var file in folderFiles)
        {
            var name = Path.GetFileName(file);
            EditorUtility.DisplayProgressBar("Unit test", "Processing level: " + name, progress);
            if (!testResult)
            {
                break;
            }

            try
            {
                OpenLevel(file, false, false, false, false);
            }
            catch (Exception ex)
            {
                testResult = false;
                UnityEngine.Debug.Log("Loading of level " + file + "failed with error " + ex.Message);
            }
            progress += step;
        }
        return testResult;
    }

public static Diablo2Editor.EditorSettings Settings()
    {
        return settings;
    }


    private static void OpenLevel(string path, bool instantiate = true, bool test_serialization = false, bool displayProgress = true, bool loadJson = true)
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
            UnityEngine.Debug.LogWarning("Level not found " + absolute_path);
        }

        if (jsonExists)
        {

            jsonContent = File.ReadAllText(pathToJson);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Preset for level " + absolute_path + " is not found in path " + pathToJson);
        }

        if (levelExists && jsonExists)
        {
            // Load preset
            loader.LoadLevel(fileName, dsContent, jsonContent, instantiate, displayProgress, loadJson);
            //Perform some tests if they are needed
            if (test_serialization)
            {
                loader.TestLevelLoading(dsContent, jsonContent);
            }
        }
    }
}
