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

    public void Init(ObjectsLoader loader, DS1Level owner, int index)
    {
        this.owner = owner;
        ds1Object = owner.objects[index];
        UpdateProperties();
        loader.Load((int)owner.act, this.type, this.id, gameObject);
        gameObject.transform.position = FromSubtileToPosition(x, y);
    }

    private void UpdateProperties()
    {
        type = (int)ds1Object.type;
        id = (int)ds1Object.id;
        x = (int)ds1Object.x;
        y = (int)ds1Object.y;
        flags = ds1Object.flags;
    }

    private Vector3 FromSubtileToPosition(int subtileX, int subtileY)
    {
        var subtileStep = CoordinateSystem.SUBTILE_GRID_STEP;
        return new Vector3(subtileStep.x * subtileX, subtileStep.y, subtileStep.z * subtileY);
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
