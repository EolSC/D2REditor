using UnityEngine;

public class DS1TaggedTile
{
    public uint num;
    public byte flags;

    public void DeepCopy(DS1TaggedTile other)
    {
        num = other.num;
        flags = other.flags;
    }
}
