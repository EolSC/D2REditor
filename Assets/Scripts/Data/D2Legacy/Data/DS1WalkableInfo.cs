using Diablo2Editor;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TileWalkableData
{
    public byte[] walkable = new byte[DT1Block.SUBTILES_COUNT];

    public void Clear()
    {
        for (int i = 0; i < walkable.Length; i++) 
        {
            walkable[i] = 0;
        }
    }
    public void MarkUnwalkable()
    {
        for (int i = 0; i < walkable.Length; i++)
        {
            walkable[i] |= 1;
        }
    }

    public void Update(byte[] subtile_flags)
    {
        bool lengthMatch = subtile_flags.Length == walkable.Length;
        Debug.Assert(lengthMatch);
        if (!lengthMatch)
        {
            return;
        }

        for (int i = 0; i < walkable.Length; i++)
        {
            walkable[i] |= subtile_flags[i];
        }
    }

}
public class DS1WalkableInfo
{

    TileWalkableData[,] walkableInfo;
    DS1Level owner;

    public TileWalkableData GetWalkableData(int x, int y)
    {
        if (x >= 0 && x < owner.width)
        {
            if (y >= 0 && y < owner.height)
            {
                return walkableInfo[y, x];
            }
        }
        return null;
    }

    public void Init(DS1Level level)
    {
        int width = (int)level.width;
        int height = (int)level.height;
        owner = level;

        walkableInfo = new TileWalkableData[height, width];
        for (int y =0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                walkableInfo[y, x] = new TileWalkableData();
                UpdateWalkableInfo(x, y);
            }
        }
    }

    public void UpdateWalkableInfo(int x, int y)
    {
        var walkableData = GetWalkableData(x, y);
        var level = owner;

        walkableData.Clear();
        int all_floor_props = 0;
        // floors
        for (int f = 0; f < level.floor.floor_num; f++)
        {
            var floorTile = level.floor.floor_array[f, y, x];
            all_floor_props |= floorTile.prop1 | floorTile.prop2 |
                               floorTile.prop3 | floorTile.prop4;
            if (!floorTile.IsWalkable())
            {
                // this is a global unwalkable info
                walkableData.MarkUnwalkable();
            }
            var block_index = floorTile.bt_idx;
            
            if (block_index > 0) // not -1 and not 0
            {
                var block = level.block_table[floorTile.bt_idx];
                int bi = block.block_idx;
                var subtile_flags = block.tileData.blocks[bi].sub_tiles_flags;

                // add the flags
                walkableData.Update(subtile_flags);                    
            }
        }

        // if no floor at all (F1 & F2 layer) the tile is completly unwalkable
        if (level.floor.floor_num == 1)
        {
            var floorTile = level.floor.floor_array[0, y, x];
            if (floorTile.prop1 == 0)
            {
                walkableData.MarkUnwalkable();
            }
        }
        else if (level.floor.floor_num == 2)
        {
            var floorTile1 = level.floor.floor_array[0, y, x];
            var floorTile2 = level.floor.floor_array[1, y, x];

            if ((floorTile1.prop1 == 0) && (floorTile2.prop1 == 0))
            {
                walkableData.MarkUnwalkable();
            }
        }

        // walls
        for (int w = 0; w < level.wall.wall_num; w++)
        {
            var wallTile = level.wall.wall_array[w, y, x];
            if (!wallTile.IsWalkable())
            {
                // this is a global unwalkable info
                walkableData.MarkUnwalkable();
            }
            var block_index = wallTile.bt_idx;
            if (block_index > 0) // not -1 and not 0
            {
                var block = level.block_table[wallTile.bt_idx];
                int bi = block.block_idx;
                var subtile_flags = block.tileData.blocks[bi].sub_tiles_flags;

                // add the flags
                walkableData.Update(subtile_flags);

                // upper / left tile corner 2nd tile
                if (wallTile.orientation == 3)
                {
                    int corner_index = SearchCorner(level, wallTile.bt_idx, block.main_index, block.sub_index);
                    if (corner_index != -1)
                    {
                        var block_corner = level.block_table[corner_index];
                        int corner_block_index = block_corner.block_idx;
                        var corner_subtile_flags = block_corner.tileData.blocks[corner_block_index].sub_tiles_flags;

                        // add the flags
                        walkableData.Update(corner_subtile_flags);
                    }
                }
            }
        }
    }

    private int SearchCorner(DS1Level level, int blockIndex, long mainIndex, long subIndex)
    {
        for (int i = blockIndex; i < level.block_table.Count; ++i)
        {
            var bOrientation = level.block_table[i].orientation;
            var bSubIndex = level.block_table[i].sub_index;
            var bMainIndex = level.block_table[i].main_index;
            if (
                (bOrientation == 4) &&
                (mainIndex == bMainIndex) &&
                (subIndex == bSubIndex)
                )
            {
                return i;
            }


        }
        return -1;
    }
}
