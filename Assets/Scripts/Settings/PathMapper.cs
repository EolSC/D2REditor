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
        // Path to .ds1 tiles in data folder. Remains the same for all users.
        private string tilesRoot = "";
        // Path to .json presets in data folder. Remains the same for all users.
        private string presetRoot = "";
        // Path to leveltypes.txt
        private string levelTypesPath = "";
        // DS1 extension
        // JSON preset extension
        private const string DS1_EXT = ".ds1";
        private const string JSON_EXT = ".json";

        /*
         * Path setup. Must be called before any work to read proper paths.
         */
        public void Init(JSONObject settings)
        {
            if (settings.IsObject)
            {
                dataRoot = Path.GetFullPath(settings["dataRoot"]);
                tilesRoot = Path.GetFullPath(Path.Combine(dataRoot, settings["tilesRoot"]));
                presetRoot = Path.GetFullPath(Path.Combine(dataRoot, settings["presetRoot"]));
                levelTypesPath = settings["levelTypes"];
            }
            ValidateDataRoot();
        }

        private void ValidateDataRoot()
        {
            if (!Directory.Exists(dataRoot))
            {
                Debug.LogError("[PathMapper] Invalid data root: " + dataRoot);
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

        public string GetTilesRoot()
        {
            return tilesRoot;
        }

        /*
         * Constructs path to .json preset based on ds1 filename
         */
        public string GetPresetForLevel(string path_to_level)
        {
            string fileName = Path.GetFileName(path_to_level);
            string jsonFileName = fileName.Replace(DS1_EXT, JSON_EXT);
            string folder = path_to_level.Replace(fileName, "");
            string json_path = Path.Combine(dataRoot, folder, jsonFileName);
            string result = json_path.Replace(tilesRoot, presetRoot);
            return result;
        }
        /*
        * Converts full path to local path within data folder
        */
        public string GetLocalPath(string absolute_path)
        {
            return absolute_path.Replace(dataRoot, "");
        }
    }
}
