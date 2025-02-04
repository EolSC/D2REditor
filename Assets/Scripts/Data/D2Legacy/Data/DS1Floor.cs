using UnityEngine;
using System.Collections.Generic;
using Diablo2Editor;
using static Unity.Collections.AllocatorManager;

public class DS1Block
{
    private const byte BIT_2 = 0x02;
    private const byte BIT_12 = 0x03;
    private const byte BIT_8 = 0x80;
    private const byte BIT_LOWER_HALF = 0x0F;

    public byte prop1;  // priority
    public byte prop2;  // sub_index
    public byte prop3;  // main_index
    public byte prop4;  // main_index + flags
    public int bt_idx;
    public byte flags;

    public int GetMainIndex()
    {
        return (prop3 >> 4) + ((prop4 & BIT_12) << 4);
    }

    public virtual void SetMainIndex(long main_index)
    {
        prop3 = (byte)((main_index & BIT_LOWER_HALF) << 4);
        prop4 = (byte)((main_index & BIT_12) >> 4);
    }

    public virtual byte GetNormalPriority()
    {
        return 0;
    }

    public void SetHidden(bool hidden)
    {
        if (hidden)
        {
            // set 8th bit
            prop4 = (byte)(prop4 | BIT_8);
        }
        else
        {
            // clear 8th bit
            prop4 = (byte)(prop4 & ~BIT_8);
        }
    }

    public void SetWalkable(bool walkable)
    {
        if (walkable)
        {
            // clear 3rd bit
            prop3 = (byte)(prop3 & ~BIT_2);
        }
        else
        {
            // set 3rd bit
            prop3 = (byte)(prop3 | BIT_2);
        }
    }

    public bool IsWalkable()
    {
        return (prop3 & BIT_2) == 0;
    }


    public bool IsHidden()
    {
        return (prop4 & BIT_8) != 0;
    }
}

public class DS1FloorCell : DS1Block
{
    const byte DEFAULT_FLOOR_PRIORITY = 194;

    public override byte GetNormalPriority()
    {
        return DEFAULT_FLOOR_PRIORITY;
    }

}

public class DS1ShadowCell : DS1Block
{
    const byte DEFAULT_SHADOW_PRIORITY = 128;
    public override byte GetNormalPriority()
    {
        return DEFAULT_SHADOW_PRIORITY;
    }

    public override void SetMainIndex(long main_index)
    {
        prop3 = (byte)((main_index & 0x0F) << 4);
        prop4 = (byte)((main_index & 0x30) >> 4);
        SetHidden(true);
    }

}

public class DS1WallCell : DS1Block
{
    public byte orientation;

    const byte DEFAULT_WALL_PRIORITY = 129;
    public override byte GetNormalPriority()
    {
        return DEFAULT_WALL_PRIORITY;
    }

    public bool IsSpecial()
    {
        return orientation == 11 || orientation == 10;
    }
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
