using SimpleJSON;
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
        private static string data_root = "";
        // Path to .ds1 tiles in data folder. Remains the same for all users.
        private static string tiles_root = "";
        // Path to .json presets in data folder. Remains the same for all users.
        private static string preset_root = "";

        // Devek=loper settings. Path to test level in data folder to speed up menuing
        private static string test_level = "";


        // Settungs file name
        private const string SETTINGS_FILE = "Settings/PathSettings.json";
        // DS1 extension
        // JSON preset extension
        private const string DS1_EXT = ".ds1";
        private const string JSON_EXT = ".json";

        /*
         * Path setup. Must be called before any work to read proper paths.
         */
        public static void InitBaseFolder()
        {
            string settings_full_path = Path.Combine(Application.dataPath, SETTINGS_FILE);
            if (File.Exists(settings_full_path))
            {

                var jsonContent = File.ReadAllText(settings_full_path);
                JSONNode settings = JSONNode.Parse(jsonContent);
                if (settings.IsObject)
                {
                    data_root = Path.GetFullPath(settings["data_root"]);
                    test_level = settings["test_level"];
                    tiles_root = Path.GetFullPath(Path.Combine(data_root, settings["tiles_root"]));
                    preset_root = Path.GetFullPath(Path.Combine(data_root, settings["preset_root"]));
                }
            }
            ValidateDataRoot();
        }

        private static void ValidateDataRoot()
        {
            if (!Directory.Exists(data_root))
            {
                Debug.LogError("[PathMapper] Invalid data root: " + data_root);
            }
        }

        /*
         * Converts local path within data folder to full path.
         */
        public static string GetAbsolutePath(string local_path)
        {
            var canonical_path = Path.GetFullPath(Path.Combine(data_root, local_path));
            return canonical_path;
        }

        public static string GetTestLevel()
        {
            return test_level;
        }

        /*
         * Constructs path to .json preset based on ds1 filename
         */
        public static string GetPresetForLevel(string path_to_level)
        {
            string fileName = Path.GetFileName(path_to_level);
            string jsonFileName = fileName.Replace(DS1_EXT, JSON_EXT);
            string folder = path_to_level.Replace(fileName, "");
            string json_path = Path.Combine(data_root, folder, jsonFileName);
            string result = json_path.Replace(tiles_root, preset_root);
            return result;
        }
        /*
        * Converts full path to local path within data folder
        */
        public static string GetLocalPath(string absolute_path)
        {
            return absolute_path.Replace(data_root, "");
        }
    }
}
