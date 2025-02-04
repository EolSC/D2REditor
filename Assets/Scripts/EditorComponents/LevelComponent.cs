using Diablo2Editor;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Mesh;

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
    // string name
    private string levelName;

    public string GetName()
    {
        return levelName;
    }

    public void Load(string name, byte[] ds1Content, string jsonContent, bool instantiate = true)
    {
        this.levelName = name;

        EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.0f);
        LoadJsonPreset(jsonContent);
        EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.3f);
        LoadDS1Content(ds1Content);
        if (instantiate)
        {
            ContentDrawer.InstantiateContent(gameObject, preset, ds1Level);
        }

        gameObject.name = levelName;
        // Flip hierarchy around X-axis
        // D2 granny models use other coordinate system. To show level in same coordinates
        // as game shows it
        var presetGO = preset.gameObject;
        presetGO.transform.localScale = new Vector3(-1, 1, 1);

        Debug.Log("Level loaded: " + levelName);
        EditorUtility.ClearProgressBar();


    }

    public void Save(string fileName)
    {
        string path = Path.GetDirectoryName(fileName);
        string name_no_ext = Path.GetFileNameWithoutExtension(fileName);

        JSONNode resultJson = preset.Serialize();
        DS1Saver saver = new DS1Saver();

        byte[] resultDS1 = saver.SaveDS1(ds1Level);

        string jsonFileName = Path.Combine(path, name_no_ext + PathMapper.JSON_EXT);
        string ds1FileName = fileName;
        StringBuilder builder = new StringBuilder();
        resultJson.WriteToStringBuilder(builder, 2, 2, JSONTextMode.Indent);
        File.WriteAllText(jsonFileName, builder.ToString());
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
                // Can't check old content - resaving it upgrades level version to 18
                // so binary content will be changed
                // Just skip it for now 'cause we don't support saving in older versions  
                oldContentEqual = true;
            }
            bool jsonEqual = JSONCompare.Compare(sourceJson, resultJson);

            return oldContentEqual && jsonEqual;
        }
        return false;
    }

    // Json-specific logic
    private void LoadJsonPreset(string jsonContent)
    {
        JSONNode jsonNode = JSON.Parse(jsonContent);
        preset = new Diablo2Editor.LevelPreset(gameObject, jsonNode.AsObject, seed);
    }

    // Ds1-specific logic
    private void LoadDS1Content(byte[] ds1Content)
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
