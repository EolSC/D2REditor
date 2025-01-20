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
    DS1FloorCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    int bt_idx;
    public byte flags;
}

public class DS1ShadowCell
{
    DS1ShadowCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    int bt_idx;
    public byte flags;
}

public class DS1WallCell
{
    DS1WallCell()
    {

    }

    public byte prop1;
    public byte prop2;
    public byte prop3;
    public byte prop4;
    public byte orientation;
    int bt_idx;
    public byte flags;
}

public class DS1TaggedCell
{
    DS1TaggedCell()
    {

    }

    public ulong num;
    public byte flags;
}
public class DS1Floor
{
    public List<DS1FloorCell> floor_buff = new List<DS1FloorCell>();  // buffer for all floor layers
    public List<DS1FloorCell> floor_buff2 = new List<DS1FloorCell>();  // 2nd buffer, for copy/paste
    public int floor_buff_len; // sizeof the floor buffer (in bytes)
    public int floor_num;      // # of layers in floor buffer
    public int floor_line;     // width * floor_num
    public int floor_len;      // floor_line * height    
}

public class DS1Shadow
{
    public List<DS1ShadowCell> shadow_buff = new List<DS1ShadowCell>();  // buffer for all shadow layers
    public List<DS1ShadowCell> shadow_buff2 = new List<DS1ShadowCell>();  // 2nd buffer, for copy/paste
    int shadow_buff_len; // sizeof the shadow buffer (in bytes)
    int shadow_num;      // # of layers in shadow buffer
    int shadow_line;     // width * shadow_num
    int shadow_len;      // shadow_line * height   
}

public class DS1Wall
{
    public List<DS1WallCell> wall_buff = new List<DS1WallCell>();  // buffer for all wall layers
    public List<DS1WallCell> wall_buff2 = new List<DS1WallCell>();  // 2nd buffer, for copy/paste
    int wall_buff_len;  // sizeof the wall buffer (in bytes)
    int wall_num;       // # of layers in wall buffer
    int wall_line;      // width * wall_num
    int wall_len;       // wall_line * height  
}


public class DS1Tagged
{
    public List<DS1TaggedCell> tag_buff = new List<DS1TaggedCell>();  // buffer for all wall layers
    public List<DS1TaggedCell> tag_buff2 = new List<DS1TaggedCell>();  // 2nd buffer, for copy/paste
    int tag_buff_len; // sizeof the unk buffer (in bytes)
    int tag_num;      // # of layers in unk buffer
    int tag_line;     // width * unk_num
    int tag_len;      // unk_line * height 
}
