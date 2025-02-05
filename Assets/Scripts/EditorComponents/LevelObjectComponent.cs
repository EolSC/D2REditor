using Diablo2Editor;
using UnityEngine;

public class LevelObjectComponent : MonoBehaviour
{
    private DS1Level owner;

    private DS1Object ds1Object;
    [SerializeField]
    private int type;
    [SerializeField]
    private int id;
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int flags;

    public void Init(DS1Level owner, int index)
    {
        this.owner = owner;
        ds1Object = owner.objects[index];
        UpdateProperties();
    }

    private void UpdateProperties()
    {
        type = (int)ds1Object.type;
        id = (int)ds1Object.id;
        x = (int)ds1Object.x;
        y = (int)ds1Object.y;
        flags = ds1Object.flags;
    }

    public DS1Object SerializeToObject()
    {
        DS1Object obj = new DS1Object();
        obj.type = type;
        obj.id = id;
        obj.x = x;
        obj.y = y;
        obj.ds1_flags = flags;

        return obj;
    }

}
