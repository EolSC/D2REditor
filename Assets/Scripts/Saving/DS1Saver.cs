using Diablo2Editor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DS1Saver
{
    public byte[] SaveDS1(DS1Level level)
    {
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);
        WriteLevel(writer, level);
        var result = stream.ToArray();
        return result;
    }

    private void WriteLevel(BinaryWriter writer, DS1Level level)
    {
        int version = 18;
        writer.Write(version);
        // next fields all must have size of 4 bytes;
        // zero based width
        writer.Write((int)(level.width - 1));
        // zero based height
        writer.Write((int)(level.height - 1));
        // act
        writer.Write((int)(level.act - 1));
        // tag
        writer.Write((int)(level.tag_type));

        //write files
        writer.Write((int)level.file_num);

        for (int i = 0; i < level.files.Count; ++i)
        {
            string fileName = level.files[i];
            char[] chars = fileName.ToCharArray();
            writer.Write(chars, 0, fileName.Length);
            writer.Write('\0');
        }

        writer.Write((int)level.wall.layers);
        writer.Write((int)level.floor.layers);

        //TileData:

        // walls
        // floors
        // shadows
        // tagged tiles
        var walls = level.wall;
        for (int n = 0; n < walls.layers; ++n)
        {
            // wall data first
            for (int y = 0; y < level.height; ++y)
            {
                for (int x = 0; x < level.width; ++x)
                {
                    var tile = walls.data[n, y, x];
                    writer.Write(tile.prop1);
                    writer.Write(tile.prop2);
                    writer.Write(tile.prop3);
                    writer.Write(tile.prop4);
                }
            }
            // then orientation
            for (int y = 0; y < level.height; ++y)
            {
                for (int x = 0; x < level.width; ++x)
                {
                    var tile = walls.data[n, y, x];
                    writer.Write(tile.orientation);
                    byte padding = 0;
                    // 3 zero bytes as padding
                    writer.Write(padding);
                    writer.Write(padding);
                    writer.Write(padding);
                }
            }

        }

        // floors
        var floors = level.floor;
        for (int n = 0; n < floors.layers; ++n)
        {
            for (int y = 0; y < level.height; ++y)
            {
                for (int x = 0; x < level.width; ++x)
                {
                    var tile = floors.data[n, y, x];
                    writer.Write(tile.prop1);
                    writer.Write(tile.prop2);
                    writer.Write(tile.prop3);
                    writer.Write(tile.prop4);
                }
            }
        }

        var shadows = level.shadow;
        for (int n = 0; n < shadows.layers; ++n)
        {
            for (int y = 0; y < level.height; ++y)
            {
                for (int x = 0; x < level.width; ++x)
                {
                    var tile = shadows.data[n, y, x];
                    writer.Write(tile.prop1);
                    writer.Write(tile.prop2);
                    writer.Write(tile.prop3);
                    writer.Write(tile.prop4);
                }
            }
        }

        if (level.tag_type > 0)
        {
            var tagged = level.tagged;
            for (int n = 0; n < tagged.layers; ++n)
            {
                for (int y = 0; y < level.height; ++y)
                {
                    for (int x = 0; x < level.width; ++x)
                    {
                        var tile = tagged.data[n, y, x];
                        writer.Write(tile.num);

                    }
                }
            }
        }

        // objects
        writer.Write((int)level.obj_num);

        List<int> npc = new List<int>();
        for (int i = 0; i < level.objects.Count; ++i)
        {
            var obj = level.objects[i];

            writer.Write((int)obj.type);
            writer.Write((int)obj.id);
            writer.Write((int)obj.x);
            writer.Write((int)obj.y);
            writer.Write((int)obj.ds1_flags);
            if (obj.path_num > 0)
            {
                npc.Add(i);
            }
        }

        // groups
        if (level.tag_type > 0)
        {
            int n = 0;
            // put 4 bytes of zeroes
            writer.Write(n);
            writer.Write((int)level.group_num);
            foreach (var gr in level.group)
            {
                writer.Write((int)gr.tile_x);
                writer.Write((int)gr.tile_y);
                writer.Write((int)gr.width);
                writer.Write((int)gr.height);
                writer.Write((int)gr.unk);
            }
        }

        // paths
        writer.Write((int)npc.Count);
        foreach (var n in npc)
        {
            var obj = level.objects[n];
            writer.Write((int)obj.path_num);
            writer.Write((int)obj.x);
            writer.Write((int)obj.y);
            for (int path = 0; path < obj.path_num; path++)
            {
                writer.Write((int)obj.paths[path].x);
                writer.Write((int)obj.paths[path].y);
                writer.Write((int)obj.paths[path].action);
            }
        }
    }
}
