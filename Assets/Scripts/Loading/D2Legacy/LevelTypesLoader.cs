using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Diablo2Editor
{
    public class LevelTypesLoader
    {
        string[][] levelTypes;

        public void Init(string pathToLevelTypes)
        {
            levelTypes = CSVReader.ReadFile(pathToLevelTypes);
        }
        public List<DT1Data> FindTilesForLevel(PathMapper pathMapper, MapListLevelData levelData, DT1Cache dt1Cache, D2Palette palettes)
        {
            List<DT1Data> result = new List<DT1Data>();
            if (levelTypes != null && levelTypes.Length >= 3)
            {
                for (int i = 2; i < levelTypes.Length; i++)
                {
                    string indexCell = levelTypes[i][1];
                    try
                    {
                        int lvlIndex = int.Parse(indexCell);
                        if (lvlIndex == levelData.lvlTypeId)
                        {
                            int actIndex = levelTypes[i].Length - 1;
                            int act = int.Parse(levelTypes[i][actIndex]);
                            var actPallette = palettes.GetPaletteForAct(act);
                            int dt1Index = 0;
                            for (int j = 2; j < levelTypes[i].Length - 1; j++)
                            {
                                string fileName = levelTypes[i][j];
                                if (fileName != "0")
                                {
                                    int maskBit = (levelData.dt1Mask >> dt1Index) & 0x01;
                                    bool needLoad = maskBit == 1;
                                    if (needLoad)
                                    {
                                        DT1Data dt1Data = null;
                                        dt1Cache.Get(fileName, ref dt1Data);
                                        if (dt1Data == null)
                                        {
                                            dt1Data = DT1Loader.ReadDT1DataFromFile(pathMapper, fileName, actPallette);
                                            dt1Cache.Add(dt1Data);
                                        }
                                        if (dt1Data != null)
                                        {
                                            result.Add(dt1Data);
                                        }
                                    }
                                }
                                dt1Index++;
                            }
                            return result;
                        }
                    }
                    catch (FormatException )
                    {
                        Debug.LogError("Incorrect dt1index " + levelData.lvlTypeId);
                        return result;

                    }
                }

            }
            else
            {
                Debug.LogError("LEvel types is not initialized. Check levelTypes in D2REditorSettings");
            }

            return result;
        }
    }

}
