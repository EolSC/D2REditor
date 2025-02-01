using Diablo2Editor;
using SimpleJSON;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
 * Component storing all level data binding D2R + D2Legacy data and GameObject together
 */
public class LevelComponent : MonoBehaviour
{
    // Loaded LevelPreset(D2R json data)
    private Diablo2Editor.LevelPreset preset = null;
    // Loaded Level(D2 level data)
    private Diablo2Editor.DS1Level ds1Level = null;
    // Random seed for level preset
    private int seed = 0;

    public void Load(string levelName, byte[] ds1Content, string jsonContent, bool instantiate = true)
    {
        EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.0f);
        LoadJsonPreset(levelName, jsonContent);
        EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.3f);
        LoadDS1Content(levelName, ds1Content);
        gameObject.name = levelName;

        if (instantiate)
        {
            ContentDrawer.InstantiateContent(gameObject, preset, ds1Level);
        }

        Debug.Log("Level loaded: " + levelName);
        EditorUtility.ClearProgressBar();

    }

    public void Save(string fileName)
    {
        string path = Path.GetDirectoryName(fileName);

        JSONNode resultJson = preset.Serialize();
        DS1Saver saver = new DS1Saver();

        byte[] resultDS1 = saver.SaveDS1(ds1Level);

        string jsonFileName = Path.Combine(path, preset.name + ".json");
        string ds1FileName = fileName;

        File.WriteAllText(jsonFileName, resultJson.ToString());
        File.WriteAllBytes(ds1FileName, resultDS1);
        Debug.Log("Level ds1 saved: " + ds1FileName);
        Debug.Log("Level json saved: " + jsonFileName);
    }

    public bool Test(byte[] ds1Content, string jsonContent)
    {
        if ((preset != null) && (ds1Level != null))
        {
            JSONNode resultJson = preset.Serialize();
            JSONNode sourceJson = JSON.Parse(jsonContent);

            bool oldContentEqual = false;
            if (ds1Level != null)
            {
                oldContentEqual = ds1Level.test_data.Equals(ds1Content);
            }
            bool jsonEqual = JSONCompare.Compare(sourceJson, resultJson);

            return oldContentEqual && jsonEqual;
        }
        return false;
    }

    // Json-specific logic
    private void LoadJsonPreset(string levelName, string jsonContent)
    {
        JSONNode jsonNode = JSON.Parse(jsonContent);
        preset = new Diablo2Editor.LevelPreset(gameObject, jsonNode.AsObject, seed);
    }

    // Ds1-specific logic
    private void LoadDS1Content(string levelName, byte[] ds1Content)
    {
        // Reload palettes
        D2Palette palletes = new D2Palette();
        palletes.LoadPalleteFiles();


        DS1Loader loader = new DS1Loader();
        ds1Level = loader.ReadDS1(ds1Content);
        LevelTypesLoader levelTypes = new LevelTypesLoader();

        // Find level data using filename including extension
        MapListLevelData levelData = EditorMain.Settings().mapList.GetLevelData(levelName + PathMapper.DS1_EXT);
        if (levelData != null)
        {
            var tileTables = levelTypes.FindTilesForLevel(levelData, palletes);
            if (tileTables.Count > 0)
            {
                ds1Level.InitBlockTable(tileTables);
            }
        }
        else
        {
            Debug.LogError("Level data not found for level " + levelName + " in maplist.csv");
        }
    }
}
