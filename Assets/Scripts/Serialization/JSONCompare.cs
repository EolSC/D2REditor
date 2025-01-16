using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{
    /*
    * Utility class to compare 2 JSONNodes recursively
    * Used in test cases to ensure Serialization/Desiralization of presets is working properly
    */
    public class JSONCompare
    {
        public static bool Compare(JSONNode first, JSONNode second)
        {
            if (first.IsObject && second.IsObject)
            {
                return CompareJsonObjects(first.AsObject, second.AsObject);
            }
            if (first.IsArray && second.IsArray)
            {
                return CompareJsonArrays(first.AsArray, second.AsArray);
            }
            // assume null objects equal
            if (first.IsNull && second.IsNull)
            {
                return true;
            }

            if (first.IsBoolean && second.IsBoolean)
            {
                return first.AsBool == second.AsBool;
            }
            if (first.IsNumber && second.IsNumber)
            {
                // Comprasion for numbers. Doesn't use inexact comprasion, works fine for tests as is.
                return CompareJsonNumbers(first, second);
            }
            if (first.IsString && second.IsString)
            {
                return first.ToString() == second.ToString();
            }

            // types are not equal - therefore they can't be equal
            return false;
        }

        private static bool CompareJsonArrays(JSONArray first, JSONArray second)
        {
            int length = first.Count;
            if (length != second.Count)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                JSONNode a = first[i];
                JSONNode b = second[i];
                if (!Compare(a, b))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool CompareJsonObjects(JSONObject first, JSONObject second)
        {
            int length = first.Count;
            if (length != second.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, JSONNode> pair in first)
            {

                JSONNode a = first[pair.Key];
                JSONNode b = second[pair.Key];
                if (!Compare(a, b))
                {
                    return false;
                }
            }
            return true;
        }


        private static bool CompareJsonNumbers(JSONNode first, JSONNode second)
        {
            return first.AsFloat == second.AsFloat;
        }
    }
}
