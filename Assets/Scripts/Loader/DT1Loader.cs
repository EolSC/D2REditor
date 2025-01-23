using UnityEngine;
using System.IO;
using System;
public class DT1Loader
{
    public static DT1Data ReadDT1DataFromFile(string pathToFile)
    { 
    
        var pathMapper = EditorMain.Settings().paths;
        string localPath = Path.Combine(pathMapper.GetTilesRoot(), pathToFile);
        string absolute_path = pathMapper.GetAbsolutePath(localPath);
        if (File.Exists(absolute_path))
        {
            DT1Data data = new DT1Data();
            data.fileName = absolute_path;
            data.content = File.ReadAllBytes(absolute_path);
            data.UpdateStructure();
            return data;


        }
        Debug.LogError("DT1 file not found: " + pathToFile);
        return null;
    }
}
