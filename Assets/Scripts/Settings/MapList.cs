using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

namespace Diablo2Editor
{

    public class MapListLevelData
    {
        public string fileName;     // DS1 file name used as ID
        public string gameName;     // string id of this file within D2
        public string path;         // path to file within global folder
        public int lvlTypeId;       // row in leveltypes which contains proper DT1 data
        public int lvlPresetId;     // row in lvlPrest.txt 
        public int dt1Mask;         // mask for DT1 files

    }
    public class MapList
    {
        List<MapListLevelData> lvlData = new List<MapListLevelData>();
        public void InitFromFile(string fileName, string pathToPresets)
        {
            if (System.IO.File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] rowData = lines[i].Split('\t');
                    if (rowData.Length == 5)
                    {
                        MapListLevelData entry = new MapListLevelData();
                        entry.fileName = rowData[0];
                        entry.gameName = rowData[1];
                        entry.path = rowData[2];
                        entry.lvlTypeId = int.Parse(rowData[3]);
                        entry.lvlPresetId = int.Parse(rowData[4]);
                        lvlData.Add(entry);

                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Maplist reading error in row " + i + " Expected 5 entries got " + rowData.Length);
                    }

                }
                ReadDT1Mask(pathToPresets);
            }
        }

        private void ReadDT1Mask(string pathToPresets)
        {
            if (System.IO.File.Exists(pathToPresets))
            {
                string[] lines = File.ReadAllLines(pathToPresets);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] row = lines[i].Split('\t');
                    string indexRow = row[1];
                    string dtMaskRow = row[row.Length - 1];
                    if (indexRow.Length > 0)
                    {
                        if (int.TryParse(dtMaskRow, out int dt1Mask))
                        {
                            if (int.TryParse(indexRow, out int index))
                            {
                                var mapData = GetLevelData(index);
                                if (mapData != null)
                                {
                                    mapData.dt1Mask = dt1Mask;
                                }
                            }
                        }
                    }
                }
            }
        }

        public MapListLevelData GetLevelData(string fileName)
        {
            foreach(var data in lvlData)
            {
                if (data.fileName == fileName)
                {
                    return data;
                }
            }
            return null;
        }

        private MapListLevelData GetLevelData(int levelPresetIndex)
        {
            foreach (var data in lvlData)
            {
                if (data.lvlPresetId == levelPresetIndex)
                {
                    return data;
                }
            }
            return null;
        }



    }

}
