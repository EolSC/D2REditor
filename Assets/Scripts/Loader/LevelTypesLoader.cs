using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Diablo2Editor
{
    public class LevelTypesLoader
    {
        public List<DT1Data> FindTilesForLevel(string pathToLevel)
        {
            List<DT1Data> result = new List<DT1Data>();
            string zone, act;
            if (GetZoneAndActFromFileName(pathToLevel, out zone, out act))
            {
                string[][] levelTypes = ReadLevelTypes();
                if (levelTypes != null && levelTypes.Length >= 3)
                {
                    for (int i = 2; i < levelTypes.Length; i++)
                    {
                        string nameCell = levelTypes[i][0];
                        string rowZone, rowAct;
                        GetZoneAndActFromLevelTypes(nameCell, out rowZone, out rowAct);
                        if (rowZone == zone &&  rowAct == act)
                        {
                            for (int j = 2; j < levelTypes[i].Length - 1; j++) 
                            {
                                string fileName = levelTypes[i][j];
                                if (fileName != "0")
                                {
                                    var dt1Data = DT1Loader.ReadDT1DataFromFile(fileName);
                                    if (dt1Data != null)
                                    {
                                        result.Add(dt1Data);
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Can't find zone and act for level " + pathToLevel);
            }

            return result;
        }

        private void GetZoneAndActFromLevelTypes(string name, out string zone, out string act)
        {
            string[] values = name.Split('-');
            if (values.Length >= 2)
            {
                act = new string(values[0].Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();
                zone = new string(values[1].Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();
            }
            else
            {
                act = "";
                zone = "";
            }
        }

        private bool GetZoneAndActFromFileName(string fileName, out string zone, out string act)
        {
            string[] parts = fileName.Split('\\');
            if (parts.Length >= 3)
            {
                // path should look like data/global/tiles/act1/barracks/barew.ds1
                // for this example array will contain [..., 'act1', 'barracks', 'barew.ds1']
                // 
                int zoneIndex = parts.Length - 2;
                int actIndex = parts.Length - 3;
                act = parts[actIndex].ToLower();
                zone = parts[zoneIndex].ToLower();
                return true;

            }
            act = "";
            zone = "";
            return false;
        }

        private string [][] ReadLevelTypes()
        {
            string[][] result = null;
            string pathToLevelTypes = EditorMain.Settings().paths.GetPathToLevelTypes();
            if (System.IO.File.Exists(pathToLevelTypes))
            { 
                string[] lines = File.ReadAllLines(pathToLevelTypes);
                result = new string[lines.Length][];
                for (int i = 0; i < lines.Length; i++) {
                    result[i] = lines[i].Split('\t');
                }
            }

            return result;
        }
    }

}
