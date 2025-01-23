using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DT1Block
{
    public long direction;
    public short roof_y;
    public byte sound;
    public byte animated;
    public long size_y;
    public long size_x;
    // long       zeros1;
    public long orientation;
    public long main_index;
    public long sub_index;
    public long rarity;
    /*
    UBYTE      unknown_a;
    UBYTE      unknown_b;
    UBYTE      unknown_c;
    UBYTE      unknown_d;
    */
    public byte[] sub_tiles_flags = new byte[25];
    // int        zeros2[7];
    public long tiles_ptr;
    public long tiles_length;
    public long tiles_number;
    // int        zeros3[12];
}

public class DT1Data
{
    public string fileName;
    public byte[] content;
    public long x1; // signature (7)
    public long x2; // signature (6)
    public long block_num;
    public long bh_start;
    public DT1Block[] blocks;
    public List<Texture2D> bitmaps = new List<Texture2D>();

    /*
     * Data offsets in DS1 structure. 
     */ 
    private class Offsets
    {
        // offsets for DS1 data
        public const int X1_OFFSET = 0;     // version headers
        public const int X2_OFFSET = 4;
        public const int BLOCK_NUM_OFFSET = 268;    // number of blocks
        public const int BLOCK_HEADER_OFFSET = 272; // block data
        public const int NEXT_BLOCK = 96;   // offset for next block in block table

        // offsets for block data
        public const int B_DIRECTION = 0;
        public const int B_ROOF = 4;
        public const int B_SOUND = 6;
        public const int B_ANIMATED = 7;
        public const int B_SIZE_Y = 8;
        public const int B_SIZE_X = 12;
        public const int B_ORIENTATION = 20;
        public const int B_MAIN_INDEX = 24;
        public const int B_SUB_INDEX = 28;
        public const int B_RARITY = 32;
        public const int B_SUBTILES_FLAGS = 40;
        public const int B_TILES = 72;
        public const int B_TILES_LENGTH = 76;
        public const int B_TILES_NUMBER = 80;
    }


    public void UpdateStructure()
    {
        if (content != null && content.Length > 0)
        {
            int streamPosition = 0;
            x1 = BitConverter.ToUInt32(content, streamPosition + Offsets.X1_OFFSET);
            x2 = BitConverter.ToUInt32(content, streamPosition + Offsets.X2_OFFSET);
            block_num = BitConverter.ToUInt32(content, streamPosition + Offsets.BLOCK_NUM_OFFSET);
            bh_start = BitConverter.ToUInt32(content, streamPosition + Offsets.BLOCK_HEADER_OFFSET);
            blocks = new DT1Block[block_num];
            UpdateBlocks();
            LoadBlockTextures();
        }
    }

    private void UpdateBlocks()
    {
        int streamPosition = (int)bh_start;
        int []idxtable = {20, 21, 22, 23, 24,
                          15, 16, 17, 18, 19,
                          10, 11, 12, 13, 14,
                           5,  6,  7,  8,  9,
                           0,  1,  2,  3,  4};

        for (int i = 0; i < block_num; i++)
        {
            var block = blocks[i];
            block.direction = BitConverter.ToUInt32(content, 
                streamPosition + Offsets.B_DIRECTION);
            block.roof_y = BitConverter.ToInt16(content,
                streamPosition + Offsets.B_ROOF);
            block.sound = content[streamPosition + Offsets.B_SOUND];
            block.animated = content[streamPosition + Offsets.B_ANIMATED];
            block.size_x = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_SIZE_X);
            block.size_y = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_SIZE_Y);
            block.orientation = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_ORIENTATION);
            block.main_index = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_MAIN_INDEX);
            block.sub_index = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_SUB_INDEX);
            block.rarity = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_RARITY);

            int subtile_flags = streamPosition + Offsets.B_SUBTILES_FLAGS;
            for (int j = 0; j < subtile_flags; j++)
            {
                int index = idxtable[j];
                block.sub_tiles_flags[index] = content[subtile_flags + j];
            }

            block.tiles_ptr = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_TILES);
            block.tiles_length = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_TILES_LENGTH);
            block.tiles_number = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_TILES_NUMBER);

            streamPosition += Offsets.NEXT_BLOCK;
        }
    }

    private void LoadBlockTextures()
    { 
    }

}
