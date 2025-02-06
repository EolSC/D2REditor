using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Diablo2Editor
{
    public class LevelTypesLoader
    {
        public List<DT1Data> FindTilesForLevel(MapListLevelData levelData, D2Palette palettes)
        {
            List<DT1Data> result = new List<DT1Data>();
            string pathToLevelTypes = EditorMain.Settings().paths.GetPathToLevelTypes();
            string[][] levelTypes = CSVReader.ReadFile(pathToLevelTypes);
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
                                        var dt1Data = DT1Loader.ReadDT1DataFromFile(fileName, actPallette);
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
                Debug.LogError("Can't lvltypes.txt: path in settings " + pathToLevelTypes);
            }

            return result;
        }
    }

}
