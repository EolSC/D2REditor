using UnityEngine;

public class DS1WallTile : DS1Tile
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

    public void DeepCopy(DS1WallTile other)
    {
        base.DeepCopy(other);
        orientation = other.orientation;
    }
}