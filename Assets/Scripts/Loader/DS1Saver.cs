using Diablo2Editor;
using System;
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

        writer.Write((int)level.wall.wall_num);
        writer.Write((int)level.floor.floor_num);

        //TODO: implement saving all the data

        //TileData:

        // walls
        // floors
        // shadows
        // tagged tiles

        // Other data:

        // objects
        // groups
        // paths


    }
}
