using SimpleJSON;
using System.IO;
using UnityEngine;
namespace Diablo2Editor
{
    public class PathMapper
    {
        private static string data_root = "";
        private static string test_level = "";
        private static string tiles_root = "";
        private static string preset_root = "";


        private const string SETTINGS_FILE = "Settings/PathSettings.json";
        private const string DS1_EXT = ".ds1";
        private const string JSON_EXT = ".json";


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
        }

        public static string GetAbsolutePath(string local_path)
        {
            var canonical_path = Path.GetFullPath(Path.Combine(data_root, local_path));
            return canonical_path;
        }

        public static string GetTestLevel()
        {
            return test_level;
        }

        public static string GetPresetForLevel(string path_to_level)
        {
            string fileName = Path.GetFileName(path_to_level);
            string jsonFileName = fileName.Replace(DS1_EXT, JSON_EXT);
            string folder = path_to_level.Replace(fileName, "");
            string json_path = Path.Combine(data_root, folder, jsonFileName);
            string result = json_path.Replace(tiles_root, preset_root);
            return result;
        }

        public static string GetLocalPath(string absolute_path)
        {
            return absolute_path.Replace(data_root, "");
        }
    }
}
