using Diablo2Editor;
using SimpleJSON;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Mesh;

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

    private ContentDrawer drawer = new ContentDrawer();

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


    public void Load(string name, byte[] ds1Content, string jsonContent, bool instantiate = true, bool displayProgress = true)
    {
        this.levelName = name;

        if (displayProgress)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.0f);
        }
        LoadJsonPreset(jsonContent);
        if (displayProgress)
        {
            EditorUtility.DisplayProgressBar("Loading level", "Loading json content...", 0.3f);
        }

        LoadDS1Content(ds1Content);
        if (instantiate)
        {
            drawer.InstantiateContent(gameObject, this, displayProgress);
        }
        if (displayProgress)
        {
            Debug.Log("Level loaded: " + levelName);
            EditorUtility.ClearProgressBar();
        }
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
    private void LoadJsonPreset(string jsonContent)
    {
        JSONNode jsonNode = JSON.Parse(jsonContent);
        GameObject jsonObj = new GameObject();
        jsonObj.name = "json";
        jsonObj.transform.parent = transform;
        d2RPreset = new Diablo2Editor.LevelPreset(jsonObj, jsonNode.AsObject, seed);
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
            ds1Level.InitBlockTable(tileTables);
        }
        else
        {
            Debug.LogError("Level data not found for level " + levelName + " in maplist.csv");
        }
        ds1Level.InitWalkableData();

    }
}
