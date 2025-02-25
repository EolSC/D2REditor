using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Diablo2Editor;
using UnityEngine.WSA;

[TestFixture]
public class SerializationTest
{
    private LevelLoadingStrategy strategy;
    private LevelContentLoader loader;


    [SetUp]
    public void SetUp()
    {
        strategy = new LevelLoadingStrategy();
        loader = new LevelContentLoader(strategy);
    }

    [TearDown]
    public void TearDown()
    {
        LevelContentLoader.ClearScene();
    }
    private async void CleanUpMemory()
    {
        // Do some cleaning 'cause Unit test can be very memory-demanding

        strategy.cache.Clear();                                 // Cache is likely to be ineffective for separate folders so it's good idea to clean it up here
        var task = Resources.UnloadUnusedAssets();     // Unused assets can also have big impact. 
        while (!task.isDone)
        {
            await task;
        }
        System.GC.Collect(0, GCCollectionMode.Forced, true);                            // Run GC to sweep all unused data
    }

    private bool UnitTestOneFolder(string folder, float step, ref float progress, SearchOption option)
    {
        CleanUpMemory();
        var loadingContext = LevelLoadingContext.GetUnitTestContext();

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
                LevelContentLoader.ClearScene();
                loader.OpenLevel(file, loadingContext);
            }
            catch (Exception ex)
            {
                testResult = false;
                UnityEngine.Debug.Log("Loading of level " + file + " failed with error " + ex.Message);
            }
            progress += step;
        }
        return testResult;
    }

    [Test]
    public void TestSerialization()
    {
        var testStartTime = Time.realtimeSinceStartup;
        // Collect all *.ds1 files in unit test dir
        var pathMapper = strategy.settings.paths;
        var developerSettings = strategy.settings.developer;
        developerSettings.isUnitTestMode = true;

        var folders = developerSettings.unitTestFolders;
        int count = 0;
        bool testResult = true;
        foreach (var folder in folders)
        {
            var testDir = Path.Combine(pathMapper.GetUnitTestRoot(), folder);
            string[] folderFiles =
            Directory.GetFiles(testDir, "*.ds1", SearchOption.AllDirectories);
            count += folderFiles.Length;
        }

        if (count > 0)
        {
            float step = 1 / (float)count;
            float progress = 0.0f;

            foreach (var file in folders)
            {
                var testDir = Path.Combine(pathMapper.GetUnitTestRoot(), file);
                // Search directory
                testResult = UnitTestOneFolder(testDir, step, ref progress, SearchOption.TopDirectoryOnly);
                if (!testResult)
                {
                    break;
                }
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
        strategy.dt1Cache.Clear();

        var testEndTime = Time.realtimeSinceStartup;
        var diffTime = testEndTime - testStartTime;

        string resultString = testResult ? "Success. " : "Failure. ";
        string elapsedTime = "Elapsed time is " + diffTime + " seconds ";
        string statsString = "Tested " + count + " levels in " + folders.Count + " folders. Test result is " + resultString + elapsedTime;
        UnityEngine.Debug.Log(statsString);
    }
}