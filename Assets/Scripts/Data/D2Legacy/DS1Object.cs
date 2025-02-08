using System.Collections.Generic;

public class DS1Object
{
    public long type;
    public long id;
    public long x;     // sub-cell X
    public long y;     // sub-cell Y
    public long ds1_flags;

    public List<DS1ObjectPath> paths = new List<DS1ObjectPath>();   
}
