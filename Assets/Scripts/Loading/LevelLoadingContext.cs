using UnityEngine;
namespace Diablo2Editor
{
    /*
     * Data used to load and instantiate the level
     * Diffrent flags can be used in different scenarios. Default loading 
     * does instantiating with no json validation test and displays progress
     */
    public class LevelLoadingContext
    {
        public string name;
        public byte[] ds1Content;
        public string jsonContent;
        public bool instantiate = true;
        public bool test = true;
        public bool loadJson = true;
        public bool displayProgress = true;
        public bool clearCache = false;

        public static LevelLoadingContext GetUnitTestContext()
        {
            var result = new LevelLoadingContext();
            result.instantiate = false;
            result.test = true;
            result.loadJson = true;
            result.displayProgress = false;
            return result;
        }

        public static LevelLoadingContext GetDefaultContext()
        {
            var result = new LevelLoadingContext();
            result.instantiate = true;
            result.test = true;
            result.loadJson = true;
            result.displayProgress = true;
            return result;
        }

    }
}
