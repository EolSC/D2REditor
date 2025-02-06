using System.IO;
using UnityEngine;

public class CSVReader
{
    public static string[][] ReadFile(string fileName)
    {
        string[][] result = null;
        if (System.IO.File.Exists(fileName))
        {
            string[] lines = File.ReadAllLines(fileName);
            result = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].Split('\t');
            }
        }

        return result;
    }
}
