using SimpleJSON;
using System.Data;
using System.IO;
using UnityEngine;
namespace Diablo2Editor
{
    /*
     * Stores data related to paths in D2R data directory
     * Handles all path conversions.
     */
    public class PathMapper
    {
        // Root folder for D2R content folder. It's user-specific setting, must be configured before any work
        private string dataRoot = "";
        // Mod root folder for D2R content folder. It's user-specific setting, must be configured before any work
        private string modRoot = "";

        // Path to .ds1 tiles in data folder. Remains the same for all users.
        private string tilesRoot = "";
        // Path to .json presets in data folder. Remains the same for all users.
        private string presetRoot = "";
        // Path to lvltypes.txt
        private string levelTypesPath = "";
        // Path to lvlprest.txt
        private string levelPresetsPath = "";
        // Path to maplist.csv
        private string mapListPath = "";
        // Palettes directory
        private string palettesPath = "";
        // Path to monsters.json storing all possible monster presets
        private string monstersPath = "";
        // Path to objects.json storing all possible environment objects presets
        private string objectsPath = "";

        // Path to superuniques.txt
        private string superuniques = "";
        // Path to monpreset.txt
        private string monpreset = "";
        // Path to objpreset.txt
        private string objpreset = "";
        // Path to objects.txt
        private string objtxt = "";
        // Path to monstats.txt
        private string monstats = "";

        // Path to npc presets folder
        private string npcRoot = "";
        // Path to enemy presets folder
        private string monsterRoot = "";
        // Path to objects presets folder
        private string objectsRoot = "";

        // DS1 extension
        // JSON preset extension
        public const string DS1_EXT = ".ds1";
        public const string JSON_EXT = ".json";

        /*
         * Root setup. Sets path to D2 resource folders. It's user-specific data so it
         * needs separate settings object
         */
        public void InitRootFolders(JSONNode settings)
        {
            if (settings.IsObject)
            {
                dataRoot = Path.GetFullPath(settings["main_folder"]);
                modRoot = Path.GetFullPath(settings["mod_folder"]);
            }
            ValidateDataRoot();
        }

        /*
         * Path setup. Must be called before any work to read proper paths.
         */
        public void Init(JSONNode settings)
        {
            if (settings.IsObject)
            {
                tilesRoot = settings["tilesRoot"];
                presetRoot =  settings["presetRoot"];
                palettesPath = settings["palettePath"];
                monstersPath = settings["monstersPath"];
                objectsPath = settings["objectsPath"];
                levelTypesPath = settings["levelTypes"];
                levelPresetsPath = settings["levelPresets"];
                mapListPath = settings["maplistPath"];
                superuniques = settings["superuniques"];
                monpreset = settings["monpreset"];
                objpreset = settings["objpreset"];
                objtxt = settings["objtxt"];
                monstats = settings["monstats"];

                npcRoot = settings["npcRoot"];
                monsterRoot = settings["monsterRoot"];
                objectsRoot = settings["objectsRoot"];

            }
            ValidateDataRoot();
        }

        private void ValidateDataRoot()
        {
            if (!Directory.Exists(dataRoot))
            {
                Debug.LogError("[PathMapper] Invalid data root: " + dataRoot);
            }
            if (!Directory.Exists(modRoot))
            {
                Debug.LogError("[PathMapper] Invalid mod root: " + modRoot);
            }

        }

        /*
         * Converts local path within data folder to full path.
         */
        public string GetAbsolutePath(string local_path)
        {
            var canonical_path = Path.GetFullPath(Path.Combine(dataRoot, local_path));
            return canonical_path;
        }
        /*
         * Returns path to Leveltypes.txt used to load tiles for ds1
         */
        public string GetPathToLevelTypes()
        {
            return GetAbsolutePath(levelTypesPath);
        }

        /*
         * Returns path to lvlprest.txt used to load tiles for ds1
         */
        public string GetPathToLevelPresets()
        {
            return GetAbsolutePath(levelPresetsPath);
        }

        /*
         * Returns path to maplist.csv used to load tiles for ds1
         * It's project's resource so we don't searh for modRoot or dataRoot
         */
        public string GetPathToMapList()
        {
            return Path.GetFullPath(mapListPath);
        }

        public string GetPalettesPath()
        {
            return GetAbsolutePath(palettesPath);
        }

        public string GetMonstersPath()
        {
            return GetAbsolutePath(monstersPath);
        }
        public string GetObjectsPath()
        {
            return GetAbsolutePath(objectsPath);
        }

        public string GetSuperUniques()
        {
            return GetAbsolutePath(superuniques);
        }
        public string GetMonPreset()
        {
            return GetAbsolutePath(monpreset);
        }
        public string GetObjPreset()
        {
            return GetAbsolutePath(objpreset);
        }

        public string GetObjTxt()
        {
            return GetAbsolutePath(objtxt);
        }

        public string GetMonStats()
        {
            return GetAbsolutePath(monstats);
        }

        public string GetNPCRoot()
        {
            return GetAbsolutePath(npcRoot);
        }

        public string GetMonsterRoot()
        {
            return GetAbsolutePath(monsterRoot);
        }
        public string GetObjectsRoot()
        {
            return GetAbsolutePath(objectsRoot);
        }

        public string GetTilesRoot()
        {
            return GetAbsolutePath(tilesRoot);
        }

        /*
         * Constructs path to .json preset based on ds1 filename
         */
        public string GetPresetForLevel(string path_to_level)
        {
            string fileName = Path.GetFileName(path_to_level);
            string jsonFileName = fileName.Replace(DS1_EXT, JSON_EXT);
            string folder = path_to_level.Replace(fileName, "");
            string json_path = Path.Combine(folder, jsonFileName);
            json_path = ReverseSlashes(json_path);
            string preset_local_path = json_path.Replace(tilesRoot, presetRoot);
            string result = GetAbsolutePath(preset_local_path);
            return result;
        }
        /*
        * Converts full path to local path within data folder
        */
        public string GetLocalPath(string absolute_path)
        {
            return absolute_path.Replace(dataRoot, "");
        }

        private string ReverseSlashes(string path)
        {
            return path.Replace("\\", "/");
        }
    }
}
