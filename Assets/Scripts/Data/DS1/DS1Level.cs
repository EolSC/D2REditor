using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/* Internal data of DS1 level. 
 * 
 */

namespace Diablo2Editor
{
    public class DS1Level
    {
        public DS1Level()
        {

        }
        public byte[] test_data; // source DS1 data for tests. TODO - delete after Save/Load is implemented

        public char[] dt1_idx = new char[DS1Consts.DT1_IN_DS1_MAX];
        public int[] dt1_mask = new int[DS1Consts.DT1_IN_DS1_MAX];
        public int txt_act;
        public List<DS1BlockTable> block_table = new List<DS1BlockTable>();
        public List<DT1Data> textureBanks;
        public int bt_num;
        public byte[] wall_layer_mask = new byte[DS1Consts.WALL_MAX_LAYER];
        public byte[] floor_layer_mask = new byte[DS1Consts.FLOOR_MAX_LAYER];
        public byte[] shadow_layer_mask = new byte[DS1Consts.SHADOW_MAX_LAYER];
        public DS1ObjectLayer objects_layer_mask;
        public byte paths_layer_mask;
        public byte walkable_layer_mask;
        public byte animations_layer_mask;
        public byte special_layer_mask;
        public int subtile_help_display;

        // from file
        public long version;
        public long tag_type;
        public long width;    // from file, +1
        public long height;   // from file, +1
        public long act;      // from file, +1

        // files in the ds1 (not used by the game)
        public long file_num;
        public List<string> files = new List<string>();

        // tiles data
        public DS1Floor floor = new DS1Floor();     // floor
        public DS1Shadow shadow = new DS1Shadow();  // shadow
        public DS1Wall wall = new DS1Wall();        // wall
        public DS1Tagged tagged = new DS1Tagged();  // tagged

        // groupData
        public long group_num;
        public int group_size;
        public List<DS1Group> group = new List<DS1Group>();

        public int tile_w;
        public int tile_h;

        /*
         Internal data. TODO - clean this up if not needed
        // internal
        ZOOM_E cur_zoom;
        int tile_w;
        int tile_h;
        int height_mul;
        int height_div;
        SCROLL_S cur_scroll;

        // screen position and size for this ds1
        WIN_PREVIEW_S own_wpreview;
        */


        // Objects data 
        public int[] drawing_order;
        public List<DS1Object> objects = new List<DS1Object>();
        public long obj_num;

        // current animated floor frame
        public int cur_anim_floor_frame;

        public void InitBlockTable(List<DT1Data> tileData)
        {
            textureBanks = tileData;
            block_table = new List<DS1BlockTable>();
            int dtIndex = 0;
            foreach (var tile in tileData)
            {
                int blockIndex = 0;
                foreach (var block in tile.blocks)
                {
                    DS1BlockTable blockTable = new DS1BlockTable();

                    blockTable.orientation = block.orientation;
                    blockTable.main_index = block.main_index;
                    blockTable.sub_index = block.sub_index;
                    blockTable.rarity = block.rarity;
                    blockTable.dt1_idx_for_ds1 = dtIndex;
                    blockTable.tileData = tile;
                    blockTable.block_idx = blockIndex;
                    blockTable.zero_line = 0;
                    blockTable.roof_y = 0;
                    // type
                    if (blockTable.orientation == 0)
                    {
                        // floor
                        blockTable.zero_line = 0;
                        if (block.animated == 0x01)
                        {
                            // animated
                            blockTable.type = DS1BlockTable.BlockType.BT_ANIMATED;
                        }
                        else
                        {
                            // static
                            blockTable.type = DS1BlockTable.BlockType.BT_STATIC;
                        }
                    }
                    else if (block.orientation == 13)
                    {
                        // shadow
                        blockTable.type = DS1BlockTable.BlockType.BT_SHADOW;
                        blockTable.zero_line = -(int)block.size_y;
                    }
                    else
                    {
                        // walls
                        if ((block.orientation == 10) || (block.orientation == 11))
                        {
                            // special
                            blockTable.type = DS1BlockTable.BlockType.BT_SPECIAL;
                            blockTable.zero_line = -(int)block.size_y;
                        }
                        else if (block.orientation == 15)
                        {
                            // roof
                            blockTable.type = DS1BlockTable.BlockType.BT_ROOF;
                            blockTable.roof_y = block.roof_y;
                            blockTable.zero_line = 0;
                        }
                        else if (block.orientation < 15)
                        {
                            // wall up
                            blockTable.type = DS1BlockTable.BlockType.BT_WALL_UP;
                            blockTable.zero_line = -(int)block.size_y;
                        }
                        else if (block.orientation > 15)
                        {
                            // wall down
                            blockTable.type = DS1BlockTable.BlockType.BT_WALL_DOWN;
                            blockTable.zero_line = 96;
                        }
                    }
                    block_table.Add(blockTable);
                    blockIndex++;
                }
                dtIndex++;
            }

            InitTilesIndexes();
        }


        private void InitTilesIndexes()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int i = 0; i < wall.wall_num; i++)
                    {
                        InitWallBlock(x, y, i);
                    }

                    for (int i = 0; i < floor.floor_num; i++)
                    {
                        InitFloorBlock(x, y, i);
                    }

                    for (int i = 0; i < shadow.shadow_num; i++)
                    {
                        InitShadowBlock(x, y, i);
                    }

                }
            }
        }

        private void InitWallBlock(int x, int y, int layerIndex)
        {
            var wallBlock = wall.wall_array[layerIndex, y, x];
            wallBlock.bt_idx = -1;
            if (wallBlock.prop1 == 0)
            {
                // no tile here
                wallBlock.bt_idx = 0;
                return;
            }
            int orientation = wallBlock.orientation;
            int mainIndex = (wallBlock.prop3 >> 4) + ((wallBlock.prop4 & 0x03) << 4);
            int subIndex = wallBlock.prop2;
            int blockIndex = 0;
            foreach (var blockType in this.block_table)
            {
                if (
                    ((int)blockType.orientation == orientation) &&
                    ((int)blockType.main_index == mainIndex) &&
                    ((int)blockType.sub_index == subIndex) 
                    )
                {
                    wallBlock.bt_idx = blockIndex;
                    return;
                }
                blockIndex++;
            }

            // trick of O=18 M=3 I=1 ---> O=19 M=3 I=0
            if ((orientation == 18) || (orientation == 19))
            {
                if (orientation == 18)
                {
                    orientation = 19;
                }
                else
                {
                    orientation = 18;
                }
            }

            blockIndex = 0;
            foreach (var blockType in this.block_table)
            {
                    if (
                        ((int)blockType.orientation == orientation) &&
                        ((int)blockType.main_index == mainIndex) &&
                        ((int)blockType.sub_index == subIndex) 
                        )
                    {
                        wallBlock.bt_idx = blockIndex;
                        return;
                    }
                    blockIndex++;
            }

            subIndex = 0;
            blockIndex = 0;
            foreach (var blockType in this.block_table)
            {
                if (
                    ((int)blockType.orientation == orientation) &&
                    ((int)blockType.main_index == mainIndex) &&
                    ((int)blockType.sub_index == subIndex) 
                    )
                {
                    wallBlock.bt_idx = blockIndex;
                    return;
                }
                blockIndex++;
            }
        }

        private void InitFloorBlock(int x, int y, int layerIndex)
        {
            var floorBlock = floor.floor_array[layerIndex, y, x];
            floorBlock.bt_idx = -1;
            if (floorBlock.prop1 == 0)
            {
                // no tile here
                floorBlock.bt_idx = 0;
                return;
            }
            int mainIndex = (floorBlock.prop3 >> 4) + ((floorBlock.prop4 & 0x03) << 4);
            int subIndex = floorBlock.prop2;
            int blockIndex = 0;
            foreach (var blockType in this.block_table)
            {
                if (
                    ((int)blockType.orientation == 0) &&
                    ((int)blockType.main_index == mainIndex) &&
                    ((int)blockType.sub_index == subIndex)
                    )
                {
                    floorBlock.bt_idx = blockIndex;
                    return;
                }
                blockIndex++;
            }
        }

        private void InitShadowBlock(int x, int y, int layerIndex)
        {
            var shadowBlock = shadow.shadow_array[layerIndex, y, x];
            shadowBlock.bt_idx = -1;
            if (shadowBlock.prop1 == 0)
            {
                // no tile here
                shadowBlock.bt_idx = 0;
                return;
            }
            int mainIndex = (shadowBlock.prop3 >> 4) + ((shadowBlock.prop4 & 0x03) << 4);
            int subIndex = shadowBlock.prop2;
            int blockIndex = 0;
            foreach (var blockType in this.block_table)
            {
                if (
                    ((int)blockType.orientation == 13) &&
                    ((int)blockType.main_index == mainIndex) &&
                    ((int)blockType.sub_index == subIndex)
                    )
                {
                    shadowBlock.bt_idx = blockIndex;
                    return;
                }
                blockIndex++;
            }
        }
    }
}

         