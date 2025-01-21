using UnityEngine;
using System.Collections.Generic;
/*
  
 typedef struct CELL_W_S
{
   UBYTE prop1;
   UBYTE prop2;
   UBYTE prop3;
   UBYTE prop4;
   UBYTE orientation;
   int   bt_idx;
   UBYTE flags;
} CELL_W_S;

typedef struct CELL_F_S
{
   UBYTE prop1;
   UBYTE prop2;
   UBYTE prop3;
   UBYTE prop4;
   int   bt_idx;
   UBYTE flags;
} CELL_F_S;

typedef struct CELL_S_S // exactly the same struct for shadow as for the floor
{
   UBYTE prop1;
   UBYTE prop2;
   UBYTE prop3;
   UBYTE prop4;
   int   bt_idx;
   UBYTE flags;
} CELL_S_S;

typedef struct CELL_T_S
{
   // assume the data is 1 dword, and not 4 different bytes
   UDWORD num;
   UBYTE  flags;
} CELL_T_S;
*/

public class DS1FloorCell
{
    public DS1FloorCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    public int bt_idx;
    public byte flags;
}

public class DS1ShadowCell
{
    public DS1ShadowCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    public int bt_idx;
    public byte flags;
}

public class DS1WallCell
{
    public DS1WallCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    public byte orientation;
    public int bt_idx;
    public byte flags;
}

public class DS1TaggedCell
{
    public DS1TaggedCell()
    {

    }

    public uint num;
    public byte flags;
}
public class DS1Floor
{
    public DS1FloorCell[,,] floor_array;  // buffer for all floor layers
    public uint floor_num;            // # of layers in floor buffer
}

public class DS1Shadow
{
    public DS1ShadowCell[,,] shadow_array; // buffer for all shadow layers
    public uint shadow_num;             // # of layers in shadow buffer
}

public class DS1Wall
{
    public DS1WallCell [,,] wall_array ;  // buffer for all wall layers
    public uint wall_num;             // # of layers in wall buffer
}


public class DS1Tagged
{
    public DS1TaggedCell[,,] tag_array;  // buffer for all wall layers
    public uint tag_num;      // # of layers in unk buffer
}
