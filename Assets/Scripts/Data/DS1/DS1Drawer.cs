using Diablo2Editor;
using System.Buffers.Text;
using System.Drawing;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static Diablo2Editor.DS1BlockTable;

public class DS1Drawer
{
    Texture2D target;
    int TARGET_WIDTH = 1024;
    int TARGET_HEIGHT = 768;


    public DS1Drawer()
    {
        target = new Texture2D(TARGET_WIDTH, TARGET_HEIGHT);
    }

    public void DrawTilesToTexture(DS1Level level)
    {
        // loop 1A : lower walls, floors, shadows of dt1
        for (int y = 0; y < level.height; y++)
        {
            int base_x = y * -level.tile_w / 2;
            int base_y = y * level.tile_h / 2;
            for (int x = 0; x < level.width; x++)
            {
                int mx = base_x + x * level.tile_w / 2;
                int my = base_y + x * level.tile_h / 2;
                DrawWalls(level, x, y, mx, my, false);      // lower walls
                DrawFloorTile(level, x, y, mx, my);     // floors
                DrawShadowTile(level, x, y, mx, my);           // shadows of dt1
                
            }
        }
    }

    public void DrawWalls(DS1Level level, int x, int y, int mx, int my, bool isUpper)
    {
        for (int n = 0; n < level.wall.wall_num; n++)
        {
            var wallTIle = level.wall.wall_array[n, y, x];
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
            int hiddenFloor = wallTIle.prop4 & 0x80;
            if (hiddenFloor != 0) // binary : 1000-0000
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

                // Copy tile to target
                DrawSpriteToTarget(texture, mx, y1);

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
                        if (block_index >= level.bt_num)
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
                        DrawSpriteToTarget(texture, mx, y1);
                    }
                }
            }
        }
    }

    public void DrawFloorTile(DS1Level level, int x, int y, int mx, int my)
    {
        for (int n = 0; n < level.floor.floor_num; n++)
        {
            var floorTile = level.floor.floor_array[n, y, x];
            // if layer is hidden - skip it
            if (level.floor_layer_mask[n] == 0)
                continue;

            int block_index = floorTile.bt_idx; // index in block table

            if (block_index == 0) // no tiles here
                continue;

            int hiddenFloor = floorTile.prop4 & 0x80;
            if (hiddenFloor != 0) // binary : 1000-0000
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
                y1 += level.tile_h; // walls are lower than floors (and than roofs) by 80 pixels
                if ((y1 + texture.height) < 0)
                    continue;

                // Copy tile to target
                DrawSpriteToTarget(texture, mx, y1);

            }
        }
    }
    public void DrawShadowTile(DS1Level level, int x, int y, int mx, int my)
    {

    }

    private void DrawSpriteToTarget(Texture2D texture, int x, int y)
    {
        // Copy tile to target
        Graphics.CopyTexture(texture, 0, 0, 0, 0, texture.width, texture.height,
            target, 0, 0, x, y);
        target.Apply();
    }

    public void Intantiate(GameObject gameObject)
    {
        var subObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        subObject.transform.parent = gameObject.transform;
        subObject.transform.localScale = new Vector3(10, 10, 10);
        subObject.transform.position = new Vector3(120, 120, 120);
        var meshRenderer = subObject.GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            var material = new UnityEngine.Material(Shader.Find("Standard"));
            if (material)
            {
                material.SetTexture("_MainTex", target);
            }
            meshRenderer.material = material;
        }
    }

}
