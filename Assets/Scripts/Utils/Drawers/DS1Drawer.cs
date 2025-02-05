using Diablo2Editor;
using UnityEngine;

/*
 * Debug component to draw legacy level content to RenderTarget
 * Not used at the moment but will allow to do some legacy tiles editing
 */

public class DS1Drawer
{
    public static void DrawTilesToTexture(DS1Level level, RenderTexture target, Vector2 pos)
    {
        RenderTexture.active = target;

        GL.Clear(true, true, UnityEngine.Color.white);
        GL.PushMatrix();                                //Saves both projection and modelview matrices to the matrix stack.
        GL.LoadPixelMatrix(0, target.width, target.height, 0);			//Setup a matrix for pixel-correct rendering.
        // loop 1A : lower walls, floors, shadows of dt1
        int start_pos_x = (int)pos.x;
        int start_pos_y = (int)pos.y;

        for (int y = 0; y < level.height; y++)
        {
            int base_x = y * -level.tile_w / 2;
            int base_y = y * level.tile_h / 2;
            for (int x = 0; x < level.width; x++)
            {
                int mx = base_x + x * level.tile_w / 2 + start_pos_x;
                int my = base_y + x * level.tile_h / 2 + start_pos_y;
                {
                   DrawWalls(level, x, y, mx, my, false);  // lower walls
                   DrawFloorTile(level, x, y, mx, my);     // floors
                   DrawShadowTile(level, x, y, mx, my);    // shadows of dt1
                   DrawWalls(level, x, y, mx, my, true);   // lower walls

                }

            }
        }
        GL.PopMatrix();
        RenderTexture.active = null;
    }

    private static void DrawWalls(DS1Level level, int x, int y, int mx, int my, bool isUpper)
    {
        for (int n = 0; n < level.wall.layers; n++)
        {
            var wallTIle = level.wall.data[n, y, x];
            // if layer is hidden - skip it
            if (level.wall_layer_mask[n] == 0)
                continue;

            // draw only lower or upper wall, according to 'upper' param
            var oreintation = wallTIle.orientation;
            if ((isUpper) && (oreintation >= 15))
                continue;
            if ((!isUpper) && (oreintation <= 15))
                continue;

            int block_index = wallTIle.bt_idx; // index in block table

            if (block_index == 0) // no tiles here
                continue;

            if (level.special_layer_mask != 0 && ((oreintation == 10) || (oreintation == 11)))
            {
                // special tile asked to draw later
                continue;
            }
            bool isHidden = wallTIle.IsHidden();
            if (isHidden) // binary : 1000-0000
            {
                // hidden
                if ((oreintation != 10) && (oreintation != 11))
                {
                    block_index = -1; // a hidden floor --> "unknown"
                }
            }
            if (block_index != -1)
            {
                var block = level.block_table[block_index];
                if ((block.type != DS1BlockTable.BlockType.BT_WALL_UP) &&
                    (block.type != DS1BlockTable.BlockType.BT_WALL_DOWN) &&
                    (block.type != DS1BlockTable.BlockType.BT_SPECIAL))
                {
                    // only walls or special tiles, but no roof
                    continue;
                }

                var texture = block.tileData.bitmaps[block.block_idx];
                var y1 = my - block.zero_line;
                y1 += level.tile_h; // walls are lower than floors (and than roofs) by 80 pixels
                if ((y1 + texture.height) < 0)
                    continue;

                // Draw tile to target
                DrawSprite(texture, mx, y1);

                // // upper-left corner
                if (block.orientation == 3)
                {
                    // search the o=4 m=m s=s
                    long m = block.main_index;
                    long s = block.sub_index;
                    bool done = false;
                    bool found = false;

                    while (!done)
                    {
                        if (block_index >= level.block_table.Count)
                            done = true;
                        else
                        {
                            var nextBlock = level.block_table[block_index + 1];
                            if (nextBlock.orientation < 4)
                            {
                                block_index++;
                            }
                            else
                            {
                                if (nextBlock.orientation == 4)
                                    if ((nextBlock.main_index == m) && (nextBlock.sub_index == s))
                                    {
                                        done = found = true;
                                        block = nextBlock;
                                    }

                            }
                        }
                    }
                    if (found == true)
                    {
                        int block_idx = block.block_idx;
                        texture = block.tileData.bitmaps[block_idx];
                        if (texture == null)
                            continue;

                        y1 = my - block.zero_line;
                        y1 += level.tile_h; // walls are lower than floors (and than roofs) by 80 pixels
                        if ((y1 + texture.height) < 0)
                            continue;
                        DrawSprite(texture, mx, y1);
                    }
                }
            }
        }
    }

    public static void DrawFloorTile(DS1Level level, int x, int y, int mx, int my)
    {
        for (int n = 0; n < level.floor.layers; n++)
        {
            var floorTile = level.floor.data[n, y, x];
            // if layer is hidden - skip it
            if (level.floor_layer_mask[n] == 0)
                continue;

            int block_index = floorTile.bt_idx; // index in block table

            if (block_index == 0) // no tiles here
                continue;

            bool isHidden = floorTile.IsHidden();
            if (isHidden) // binary : 1000-0000
            {
                // hidden
                block_index = -1; // a hidden floor --> "unknown"
            }
            if (block_index != -1)
            {
                var block = level.block_table[block_index];
                if ((block.type != DS1BlockTable.BlockType.BT_STATIC) &&
                    (block.type != DS1BlockTable.BlockType.BT_ANIMATED))
                {
                    // // only floors
                    continue;
                }

                if (block.type == DS1BlockTable.BlockType.BT_ANIMATED)
                {
                    // TODO update block for proper anim frame
                }

                var texture = block.tileData.bitmaps[block.block_idx];
                var y1 = my - block.zero_line;
                if ((y1 + texture.height) < 0)
                    continue;

                // Copy tile to target
                DrawSprite(texture, mx, y1);

            }
        }
    }
    public static void DrawShadowTile(DS1Level level, int x, int y, int mx, int my)
    {

    }

    private static void DrawSprite(Texture2D texture, int x, int y)
    {
        Rect rect = new Rect(x, y, texture.width, texture.height);
        Graphics.DrawTexture(rect, texture);
    }
}
