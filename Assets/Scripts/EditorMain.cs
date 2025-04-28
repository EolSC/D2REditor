using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Diablo2Editor;

/*
 * Static data. Settings and global classes are here
 */

string editorVersion = "1.0.0";

namespace Diablo2Editor
{

}
/* 
 * Entry-point class for this project. Provides Menu items, defines basic functionality of editor
 */

public class EditorMain : MonoBehaviour
{
    private static LevelLoadingStrategy strategy = new LevelLoadingStrategy();
    private static LevelContentLoader loader = new LevelContentLoader(strategy);


    [MenuItem("Diablo Level Editor/Open level...")]
    private static void OpenLevel()
    {
        // Open file dialog
        string absolute_path = EditorUtility.OpenFilePanel("Open Diablo 2 Ressurected level", "", "ds1");
        // Load level with no tests
        var context = LevelLoadingContext.GetDefaultContext();
        loader.OpenLevel( absolute_path, context);
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
        // Load level with no tests
        var context = LevelLoadingContext.GetDefaultContext();
        loader.OpenTestLevel(context);
    }

    [MenuItem("Diablo Level Editor/Developer/Reload settings")]
    private static void ReloadSettings()
    {
       strategy.settings.Reload();
    }

    [MenuItem("Diablo Level Editor/Developer/UnitTestFolder")]
    private static void UnitTestFolder()
    {
        SerializationTest test = new SerializationTest();
        test.SetUp();
        test.TestSerialization();
        test.TearDown();
    }

}
