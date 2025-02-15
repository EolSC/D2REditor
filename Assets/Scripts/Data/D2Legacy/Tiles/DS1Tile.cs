/*
 * Base class for DS1 Tile data
 * Wall, Floor, Shadow tiles share it's data and methods
 */
public class DS1Tile
{
    private const byte BIT_2 = 0x02;
    private const byte BIT_12 = 0x03;
    private const byte BIT_56 = 0x30;
    private const byte BIT_8 = 0x80;
    private const byte BIT_LOWER_HALF = 0x0F;

    public byte prop1;  // priority
    public byte prop2;  // sub_index
    public byte prop3;  // main_index
    public byte prop4;  // main_index + flags
    public int bt_idx;

    public DS1Tile()
    {
        prop1 = GetNormalPriority();
        prop2 = 0;
        prop3 = 0;
        prop4 = 0;
        bt_idx = 0;
    }

    public int GetMainIndex()
    {
        return (prop3 >> 4) + ((prop4 & BIT_12) << 4);
    }

    public virtual void SetMainIndex(long main_index)
    {
        prop3 = (byte)((main_index & BIT_LOWER_HALF) << 4);
        prop4 = (byte)((main_index & BIT_56) >> 4);
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

    public void DeepCopy(DS1Tile other)
    {
        prop1 = other.prop1;
        prop2 = other.prop2;
        prop3 = other.prop3;
        prop4 = other.prop4;
        bt_idx  = other.bt_idx;
    }
}
