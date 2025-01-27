using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Collections;
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

public class DT1SubTile
{
    public short x_pos;
    public short y_pos;
    //   WORD  unknown1;
    public int x_grid;
    public int y_grid;
    public short format;
    public long length;
    //   WORD  unknown2;
    public long data_offset;
}

public class DT1Data
{
    public NativeArray<Color> palette;
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
            blocks[i] = new DT1Block();
            var block = blocks[i];
            block.direction = BitConverter.ToUInt32(content, 
                streamPosition + Offsets.B_DIRECTION);
            block.roof_y = BitConverter.ToInt16(content,
                streamPosition + Offsets.B_ROOF);
            block.sound = content[streamPosition + Offsets.B_SOUND];
            block.animated = content[streamPosition + Offsets.B_ANIMATED];
            block.size_x = BitConverter.ToUInt32(content,
                streamPosition + Offsets.B_SIZE_X);
            block.size_y = BitConverter.ToInt32(content,
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
            for (int j = 0; j < 25; j++)
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
        int block_index = 0;
        bitmaps = new List<Texture2D>();
        foreach(var block in blocks)
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            bitmaps.Add(texture);

            long orientation = block.orientation;
            int width = (int)block.size_x;
            // height is always negative
            int height = -(int)block.size_y;

            // adjustment (which y line in the bitmap is the zero line ?)

            // by default, when orientation > 15 : lower wall
            long y_add = 96;
            if ((orientation == 0) || (orientation == 15)) // floor or roof
            {
                if (block.size_y != 0)
                {
                    block.size_y = -80;
                    height = 80;
                    y_add = 0;
                }
            }
            else if (orientation < 15) // upper wall, shadow, special
            {
                if (block.size_y != 0)
                {
                    block.size_y += 32;
                    height -= 32;
                    y_add = height;
                }
            }

            // anti-bug (for empty block)
            if ((width == 0) || (height == 0))
            {
                continue;
            }

            block_index++;
            // normal block (non-empty)
            texture.Reinitialize(width, height);
            Color32[] colors = new Color32[width * height];
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i].r = 0;
                colors[i].g = 0;
                colors[i].b = 0;
                colors[i].a = 0;
            }
            texture.SetPixels32(colors);
            texture.Apply();
            for (int s_index = 0; s_index < block.tiles_number; s_index++) // for each sub-tiles
            {
                var subTile = LoadSubTile((int)block.tiles_ptr, s_index);
                int x0 = subTile.x_pos;
                int y0 = (int)y_add+ subTile.y_pos;
                int subtileData = (int)(block.tiles_ptr + subTile.data_offset);
                int length = (int)subTile.length;
                int format = subTile.format;

                // draw the sub-tile
                if (format == 0x0001)
                {
                    CreateSubtileIsometric(texture, x0, y0, subtileData, length, palette);

                }
                else
                {
                    CreateSubtileNormal(texture, x0, y0, subtileData, length, palette);
                }
            }
            texture.Apply();
        }
    }

    public DT1SubTile LoadSubTile(int tiles_ptr, int subtileIndex)
    {
        DT1SubTile result = new DT1SubTile();
        int streamPosition = tiles_ptr + (20 * subtileIndex);
        result.x_pos = BitConverter.ToInt16(content, streamPosition);
        result.y_pos = BitConverter.ToInt16(content, streamPosition + 2);
        // skip 2 bytes : unknown1
        result.x_grid = content[streamPosition + 6];
        result.y_grid = content[streamPosition + 7];
        result.format = BitConverter.ToInt16(content, streamPosition + 8);
        result.length = BitConverter.ToInt32(content, streamPosition + 10);
        // skip 2 bytes : unknown2
        result.data_offset = BitConverter.ToInt32(content, streamPosition + 16);

        return result;
    }

    void CreateSubtileIsometric(Texture2D texture,  int x0, int y0, int dataPosition, int length, NativeArray<Color> palette) 
    {
        int x, y = 0;
        int dataPointer = dataPosition;
        int[] xjump = { 14, 12, 10, 8, 6, 4, 2, 0, 2, 4, 6, 8, 10, 12, 14 };
        int[] nbpix = { 4, 8, 12, 16, 20, 24, 28, 32, 28, 24, 20, 16, 12, 8, 4 };

        // 3d-isometric subtile is 256 bytes, no more, no less
        if (length != 256)
            return;

        // draw
        while (length > 0)
        {
            x = xjump[y];
            int n = nbpix[y];
            length -= n;
            while (n > 0)
            {
                byte colorIndex = content[dataPointer];
                WritePixel(texture, x0 + x, y0 + y, colorIndex, palette);
                dataPointer++;
                x++;
                n--;
            }
            y++;
        }
    }
    void CreateSubtileNormal(Texture2D texture, int x0, int y0, int dataPosition, int length, NativeArray<Color> palette)
    {
        int dataPointer = dataPosition;
        int x = 0, y = 0;

        // draw
        while (length > 0)
        {
            byte b1 = content[dataPointer];
            byte b2 = content[dataPointer + 1];

            dataPointer += 2;
            length -= 2;
            if ((b1 != 0) || (b2 != 0))
            {
                x += b1;
                length -= b2;
                while (b2 != 0)
                {
                    byte colorIndex = content[dataPointer];
                    WritePixel(texture, x0 + x, y0 + y, colorIndex, palette);
                    dataPointer++;
                    x++;
                    b2--;
                }
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    public void WritePixel(Texture2D texture, int x, int y, byte colorIndex, NativeArray<Color> pal)
    {
        // Read color from palllet, assert it has 256 colors 
        Assert.AreEqual(pal.Length, 256);
        Color color = pal[colorIndex];
        texture.SetPixel(x, -y, color);
    }
}
