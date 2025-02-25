using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace Diablo2Editor
{

    class VirtualFileSystem
    {
        SortedDictionary<int, string> fileSystems = new SortedDictionary<int, string>();

        public VirtualFileSystem()
        {
        }

        public void AddFileSystem(string root, int priority)
        {
            fileSystems.Add(priority, root);
        }

        public void Clear()
        {
            fileSystems.Clear();
        }

        public string GetAbsolutePath(string path)
        {
            // traverse the roots, higher priority first
            foreach (var priority in fileSystems.Keys.Reverse())
            {
                var root = fileSystems[priority];
                // try to add root to local_path
                var abs_path = Path.Combine(root, path);
                // if file found - it's our match
                // Files in systems with higher priority will overwrite ones with lower
                if (File.Exists(abs_path))
                {
                    // is file
                    return abs_path;
                }
            }
            return "";
        }

        public string GetLocalPath(string absolute_path)
        {
            // traverse the roots, higher priority first
            foreach (var priority in fileSystems.Keys.Reverse())
            {
                var root = fileSystems[priority];
                if (absolute_path.StartsWith(root))
                {
                    return ReverseSlashes(absolute_path.Replace(root, ""));
                }
            }
            return "";
        }

        private string ReverseSlashes(string path)
        {
            return path.Replace("\\", "/");
        }

        public bool Validate()
        {
            foreach (var root in fileSystems.Values)
            {
                if (!Directory.Exists(root))
                {
                    Debug.LogError("[PathMapper] Invalid data root: " + root);
                    return false;
                }
            }
            return true;
        }
    }
    /*
     * Stores data related to paths in D2R data directory
     * Handles all path conversions.
     */
    public class PathMapper
    {
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
        private string palettesRoot = "";
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

        private VirtualFileSystem fileSystem = new VirtualFileSystem();

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
                fileSystem.Clear();

                var dataRoot = Path.GetFullPath(settings["main_folder"]);
                var modRoot = Path.GetFullPath(settings["mod_folder"]);
                
                fileSystem.AddFileSystem(dataRoot, 0);
                fileSystem.AddFileSystem(modRoot, 1);
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


                palettesRoot = settings["palettesRoot"];
                npcRoot = settings["npcRoot"];
                monsterRoot = settings["monsterRoot"];
                objectsRoot = settings["objectsRoot"];
                tilesRoot = settings["tilesRoot"];
                presetRoot = settings["presetRoot"];

            }
            ValidateDataRoot();
        }

        private void ValidateDataRoot()
        {
           fileSystem.Validate();
        }

        /*
         * Converts local path within data folder to full path.
         */
        public string GetAbsolutePath(string local_path)
        {
            return fileSystem.GetAbsolutePath(local_path);
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
            return npcRoot;
        }

        public string GetMonsterRoot()
        {
            return monsterRoot;
        }
        public string GetObjectsRoot()
        {
            return objectsRoot;
        }

        public string GetTilesRoot()
        {
            return tilesRoot;
        }

        public string GetPalettesRoot()
        {
            return palettesRoot;
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
            string preset_local_path = json_path.Replace(tilesRoot, presetRoot);
            string result = GetAbsolutePath(preset_local_path);
            return result;
        }
        /*
        * Converts full path to local path within data folder
        */
        public string GetLocalPath(string absolute_path)
        {
            return fileSystem.GetLocalPath(absolute_path);
        }
    }
}
