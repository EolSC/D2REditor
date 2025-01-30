using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Diablo2Editor
{
    public class LevelTypesLoader
    {
        public List<DT1Data> FindTilesForLevel(DS1LevelInfo info, D2Palette palettes)
        {
            List<DT1Data> result = new List<DT1Data>();
            string[][] levelTypes = ReadLevelTypes();
            if (levelTypes != null && levelTypes.Length >= 3)
            {
                for (int i = 2; i < levelTypes.Length; i++)
                {
                    string indexCell = levelTypes[i][1];
                    try
                    {
                        int lvlIndex = int.Parse(indexCell);
                        if (lvlIndex == info.dt1Index)
                        {
                            var actPallette = palettes.GetPaletteForAct(info.act);
                            int dt1Index = 0;
                            for (int j = 2; j < levelTypes[i].Length - 1; j++)
                            {
                                string fileName = levelTypes[i][j];
                                if (fileName != "0")
                                {
                                    int maskBit = (info.dt1Mask >> dt1Index) & 0x01;
                                    bool needLoad = maskBit == 1;
                                    if (needLoad)
                                    {
                                        var dt1Data = DT1Loader.ReadDT1DataFromFile(fileName, actPallette);
                                        if (dt1Data != null)
                                        {
                                            result.Add(dt1Data);
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("DT1 " + fileName + "skipped because of level mask");
                                    }
                                }
                                dt1Index++;
                            }
                            return result;
                        }
                    }
                    catch (FormatException )
                    {
                        Debug.LogError("Incorrect dt1index " + info.dt1Index);
                        return result;

                    }
                }

            }
            else
            {
                Debug.LogError("Can't find zone and act for level " + info.path);
            }

            return result;
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
