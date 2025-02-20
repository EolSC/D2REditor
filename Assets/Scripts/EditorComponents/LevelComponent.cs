using Diablo2Editor;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LevelLoadingStrategy
{
    public Diablo2Editor.EditorSettings settings;
    public ResourceCache cache;
    public DT1Cache dt1Cache;

    public LevelLoadingStrategy()
    {
        settings = new Diablo2Editor.EditorSettings();
        cache = new ResourceCache();
        dt1Cache = new DT1Cache();
    }
}
/*
 * Component storing all level data.
 * Binds D2R + D2Legacy data and GameObject together 
 * Actual instantiating is delegated to ComponentDrawer.
 * Saving and loading is also performed by LevelComponent
 */
public class LevelComponent : MonoBehaviour
{
    // Loaded LevelPreset(D2R json data)
    private Diablo2Editor.LevelPreset d2RPreset = null;
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

    public DS1Level GetDS1Level()
    {
        return ds1Level;
    }

    public LevelPreset GetD2RLevel()
    {
        return d2RPreset;
    }


    public void Load(LevelLoadingContext context, LevelLoadingStrategy strategy)
    {
        this.levelName = context.name;
        if (context.displayProgress)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.0f);
        }
        if (context.loadJson)
        {
            LoadJsonPreset(context.jsonContent, strategy);
        }
        if (context.displayProgress)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.3f);
        }

        LoadDS1Content(context.ds1Content, strategy);
    }

    public void Save(string fileName)
    {
        string path = Path.GetDirectoryName(fileName);
        string name_no_ext = Path.GetFileNameWithoutExtension(fileName);

        JSONNode resultJson = d2RPreset.Serialize();
        DS1Saver saver = new DS1Saver();
        UpdateObjectsData(ds1Level);
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

    private void UpdateObjectsData(DS1Level level)
    {
        level.objects.Clear();
        var objectsComponents = gameObject.GetComponentsInChildren<LevelObjectComponent>();
        foreach(var obj in objectsComponents)
        {
            var ds1Object = obj.SerializeToObject();
            level.objects.Add(ds1Object);
        }
    }

    public bool Test(byte[] ds1Content, string jsonContent)
    {
        if ((d2RPreset != null) && (ds1Level != null))
        {
            if (!d2RPreset.IsValid())
            {
                // Fail the test if preset is missing some components
                return false;
            }
            JSONNode resultJson = d2RPreset.Serialize();
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
    private void LoadJsonPreset(string jsonContent, LevelLoadingStrategy strategy)
    {
        JSONNode jsonNode = JSON.Parse(jsonContent);
        GameObject jsonObj = new GameObject();
        jsonObj.name = "json";
        jsonObj.transform.parent = transform;
        d2RPreset = new LevelPreset(jsonObj, strategy, jsonNode.AsObject, seed);
    }

    // Ds1-specific logic
    private void LoadDS1Content(byte[] ds1Content, LevelLoadingStrategy strategy)
    {
        var settings = strategy.settings;
        PathMapper pathMapper = strategy.settings.paths;
        D2Palette pallete = settings.pallete;
        DT1Cache dt1Cache = strategy.dt1Cache;
        DS1Loader loader = new DS1Loader();
        ds1Level = loader.ReadDS1(ds1Content);
        LevelTypesLoader levelTypes = settings.levelTypesLoader;

        // Find level data using filename including extension
        MapListLevelData levelData = settings.mapList.GetLevelData(levelName + PathMapper.DS1_EXT);
        if (levelData != null)
        {
            var tileTables = levelTypes.FindTilesForLevel(pathMapper, levelData, dt1Cache, pallete);
            ds1Level.InitBlockTable(tileTables);
        }
        else
        {
            Debug.LogError("Level data not found for level " + levelName + " in maplist.csv");
        }
        ds1Level.InitWalkableData();

    }
}
