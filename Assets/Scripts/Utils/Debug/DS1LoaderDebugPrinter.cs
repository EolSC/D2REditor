using Diablo2Editor;
using System.IO;
using UnityEngine;

namespace Diablo2Editor
{
    public class DS1LoaderDebugPrinter
    {
        StreamWriter writer = null;
        public void PrintDebugInfo(DS1Level level)
        {
            writer = File.CreateText("DS1Loader debug log.txt");
            PrintHeaderData(level);
            PrintFilesData(level);
            PrintLayersCountData(level);
            PrintDebugTilesData(level);
            PrintDebugLevelObjects(level);
            writer.Close();

        }

        private void WriteString(string s)
        {
            if (writer != null)
            {
                writer.WriteLine(s);
            }
        }

        private void PrintHeaderData(DS1Level level)
        {
            WriteString("DS1 level version " + level.version);
            WriteString("DS1 level width " + level.width);
            WriteString("DS1 level height " + level.height);
            WriteString("DS1 level act " + level.act);
            WriteString("DS1 level tag type " + level.tag_type);
        }

        private void PrintFilesData(DS1Level level)
        {
            WriteString("Number of files in level  is " + level.file_num);

            int index = 0;
            foreach (var file in level.files)
            {
                index++;
                WriteString("Name of file " + index + " is `" + file + "`");
            }
        }

        private void PrintLayersCountData(DS1Level level)
        {
            WriteString("Number of wall tiles: " + level.wall.layers);
            WriteString("Number of floor tiles: " + level.floor.layers);
            WriteString("Number of shadow tiles: " + level.shadow.layers);
            WriteString("Number of tag tiles: " + level.tagged.layers);
        }

        private void PrintDebugTilesData(DS1Level level)
        {
            uint wall_num = level.wall.layers;
            for (int n = 0; n < wall_num; n++)
            {
                WriteString("Wall layer " + n);
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        WriteString("Wall tile [ " + y + ", " + x + "]:");
                        PrintDebugWallData(level.wall.data[n, y, x]);
                    }
                }
            }

            uint floor_num = level.floor.layers;
            for (int n = 0; n < floor_num; n++)
            {
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        WriteString("Floor tile [ " + y + ", " + x + "]:");
                        PrintDebugFloorData(level.floor.data[n, y, x]);
                    }
                }
            }

            uint shadow_num = level.shadow.layers;
            for (int n = 0; n < shadow_num; n++)
            {
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        WriteString("Shadow tile [ " + y + ", " + x + "]:");
                        PrintDebugShadowData(level.shadow.data[n, y, x]);
                    }
                }
            }
            uint tags_num = level.tagged.layers;
            for (int n = 0; n < tags_num; n++)
            {
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        WriteString("Tagged tile [ " + y + ", " + x + "]:");
                        PrintDebugTaggedData(level.tagged.data[n, y, x]);
                    }
                }
            }
        }

        private void PrintDebugWallData(DS1WallTile tile)
        {
            WriteString("Prop1: " + tile.prop1);
            WriteString("Prop2: " + tile.prop2);
            WriteString("Prop3: " + tile.prop3);
            WriteString("Prop4: " + tile.prop4);
            WriteString("Orientation: " + tile.orientation);
        }

        private void PrintDebugFloorData(DS1FloorTile tile)
        {
            WriteString("Prop1: " + tile.prop1);
            WriteString("Prop2: " + tile.prop2);
            WriteString("Prop3: " + tile.prop3);
            WriteString("Prop4: " + tile.prop4);
        }
        private void PrintDebugShadowData(DS1ShadowTile tile)
        {
            WriteString("Prop1: " + tile.prop1);
            WriteString("Prop2: " + tile.prop2);
            WriteString("Prop3: " + tile.prop3);
            WriteString("Prop4: " + tile.prop4);
        }

        private void PrintDebugTaggedData(DS1TaggedTile tile)
        {
            WriteString("Num: " + tile.num);
        }

        private void PrintDebugLevelObjects(DS1Level level)
        {
            WriteString("Objects count: " + level.obj_num);
            foreach(var levelObject in level.objects)
            {
                PrintDebugObjectData(levelObject);
            }
        }

        private void PrintDebugObjectData(DS1Object obj)
        {
            WriteString("Type: " + obj.type);
            WriteString("ID: "   + obj.id);
            WriteString("X: "   + obj.x);
            WriteString("Y: "   + obj.y);
            foreach (var path in obj.paths)
            {
                PrintDebugPathData(path);
            }

        }

        private void PrintDebugPathData(DS1ObjectPath path)
        {
            WriteString("Path x: " + path.x);
            WriteString("Path y: " + path.y);
            WriteString("Path action: " + path.action);
        }
    }
}
