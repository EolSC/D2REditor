using System.IO;
using UnityEngine;

public class CSVReader
{
    public static string[][] ReadFile(string fileName, bool skipExpansionHeader = true)
    {
        string[][] result = null;
        if (System.IO.File.Exists(fileName))
        {
            int expansionIndex = -1;
            if(skipExpansionHeader)
            {
                string[] linesInput = File.ReadAllLines(fileName);
                for (int i = 0; i < linesInput.Length; i++)
                {
                    var row = linesInput[i].Split('\t');
                    if (row[0] == "Expansion")
                    {
                        expansionIndex = i;
                        break;
                    }
                }
            }

            string[] lines = File.ReadAllLines(fileName);
            if (expansionIndex >= 0)
            {
                result = new string[lines.Length - 1][];
            }
            else
            {
                result = new string[lines.Length][];
            }
            int resultIndex = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i != expansionIndex)
                {
                    result[resultIndex] = lines[i].Split('\t');
                    resultIndex++;
                }
            }

        }

        return result;
    }
}
