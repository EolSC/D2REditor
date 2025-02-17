using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using Diablo2Editor;
public class DT1Loader
{
    public static DT1Data ReadDT1DataFromFile(PathMapper pathMapper, string pathToFile, NativeArray<Color> palette)
    { 
        string localPath = Path.Combine(pathMapper.GetTilesRoot(), pathToFile);
        string absolute_path = pathMapper.GetAbsolutePath(localPath);
        if (File.Exists(absolute_path))
        {
            DT1Data data = new DT1Data();
            data.fileName = pathToFile;
            data.content = File.ReadAllBytes(absolute_path);
            data.UpdateStructure(palette);
            return data;


        }
        Debug.LogError("DT1 file not found: " + pathToFile);
        return null;
    }
}
