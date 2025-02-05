using UnityEngine;
/*
 * DS1 Shadow tile 
 */
public class DS1ShadowTile : DS1Tile
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
