using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DS1Object
{
    public long type;
    public long id;
    public long x;     // sub-cell X
    public long y;     // sub-cell Y
    public long ds1_flags;

    public List<DS1ObjectPath> paths = new List<DS1ObjectPath>();   
    public long path_num;
    public int desc_idx;
    public int flags;
    public DS1ObjectLabel label = new DS1ObjectLabel();

    // for moving
    public long old_x;
    public long old_y;

    // for sorting
    public long tx; // tile X
    public long ty; // tile Y
    public long sx; // sub-tile X
    public long sy; // sub-tile Y

    // random starting animation frame
    public byte frame_delta;
}
