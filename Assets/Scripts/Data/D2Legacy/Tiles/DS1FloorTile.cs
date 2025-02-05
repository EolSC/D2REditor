
/*
 * DS1 Floor tile
 */
public class DS1FloorTile : DS1Tile
{
    const byte DEFAULT_FLOOR_PRIORITY = 194;

    public override byte GetNormalPriority()
    {
        return DEFAULT_FLOOR_PRIORITY;
    }

}
