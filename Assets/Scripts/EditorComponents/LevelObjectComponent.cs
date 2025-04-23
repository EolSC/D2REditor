using Diablo2Editor;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class ObjectData
{
    [SerializeField]
    public int type;
    [SerializeField]
    public int id;
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public int flags;
    public bool IsEqual(ObjectData other)
    {
        return x == other.x && y == other.y 
            && id == other.id && type == other.type && flags == other.flags;
    }
    public void Copy(ObjectData other)
    {
        this.type = other.type;
        this.id = other.id;
        this.x = other.x;
        this.y = other.y;
        this.flags = other.y;
    }

}
[Serializable]
public class ObjectPathPoint
{
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public int action;

    public ObjectPathPoint(int x, int y, int action)
    {
        this.x = x;
        this.y = y;
        this.action = action;
    }

    public bool IsEqual(ObjectPathPoint point)
    {
        return x == point.x && y == point.y && action == point.action;
    }

}
[ExecuteInEditMode]
public class LevelObjectComponent : MonoBehaviour
{
    private DS1Level owner;
    private ObjectsLoader loader;
    private DS1Object ds1Object;

    [SerializeField]
    ObjectData data = new ObjectData();
    ObjectData oldData = new ObjectData();

    [SerializeField]
    private List<ObjectPathPoint> points = new List<ObjectPathPoint>();

    private List<ObjectPathPoint> oldpoints = new List<ObjectPathPoint>();

    public void Init(ObjectsLoader loader, DS1Level owner, int index)
    {
        this.owner = owner;
        this.loader = loader;
        ds1Object = owner.objects[index];
        UpdateProperties();
        oldData.Copy(data);
        oldpoints.Clear();
        foreach(var p in points)
        {
            oldpoints.Add(new ObjectPathPoint(p.x, p.y, p.action));
        }

        loader.Load((int)owner.act, this.data.type, this.data.id, gameObject, false);
        UpdatePositions();
    }
    void Update()
    {
        bool pathChanged = IsPathChanged();
        bool objectChanged = !oldData.IsEqual(data);
        if (objectChanged)
        {
            if ((oldData.type != data.type) || (oldData.id != data.id))
            {
                if (loader != null)
                {
                    loader.Load((int)owner.act, data.type, data.id, gameObject, true);
                }
            }
            if ((oldData.x != data.x) || (oldData.y != data.y))
            {
                gameObject.transform.localPosition = FromSubtileToPosition(data.x, data.y);
            }
            oldData.Copy(data);

        }
        if (transform.hasChanged)
        {
            UpdatePosFromTransform();
        }
        if (pathChanged)
        {
            foreach (var p in points)
            {
                ClampPositionByLevel(ref p.x, ref p.y);
            }
            oldpoints.Clear();
            foreach (var p in points)
            {
                oldpoints.Add(new ObjectPathPoint(p.x, p.y, p.action));
            }
        }
        if (pathChanged || transform.hasChanged)
        {
            UpdatePositions();
        }
    }

    private void UpdatePositions()
    {
        gameObject.transform.localPosition = FromSubtileToPosition(data.x, data.y);
        gameObject.transform.hasChanged = false;
        if (ds1Object.paths.Count > 0)
        {
            UpdatePaths();
        }
    }

    private void UpdatePosFromTransform()
    {
        var subtileStep = CoordinateSystem.SUBTILE_GRID_STEP;
        var pos = gameObject.transform.localPosition;
        int newSubtileX = (int)System.Math.Floor(pos.x / subtileStep.x);
        int newSubtileY = (int)System.Math.Floor(pos.z / subtileStep.z);

        ClampPositionByLevel(ref newSubtileX, ref newSubtileY);
        data.x = newSubtileX;
        data.y = newSubtileY;
    }

    public void ClampPositionByLevel(ref int x, ref int y)
    {
        if (owner == null)
        {
            return;
        }

        int maxSubtileX = (int)owner.width * DT1Block.SUBTILES_X;
        int maxSubtileY = (int)owner.height * DT1Block.SUBTILES_Y;

        if (x < 0)
        {
            x = 0;
        }
        if (x > maxSubtileX)
        {
            x = maxSubtileX;
        }
        if (y < 0)
        {
            y = 0;
        }
        if (y > maxSubtileY)
        {
            y = maxSubtileY;
        }
    }

    private bool IsPathChanged()
    {
        if (points.Count == 0 && oldpoints.Count == 0)
        {
            return false;
        }

        if (oldpoints.Count != points.Count)
        {
            return true;
        }
        for (int i = 0; i < oldpoints.Count; i++) { 
            if (!oldpoints[i].IsEqual(points[i]))
            {
                return true;
            }    
        }
        return false;
    }


    private void UpdateProperties()
    {
        data.type = (int)ds1Object.type;
        data.id = (int)ds1Object.id;
        data.x = (int)ds1Object.x;
        data.y = (int)ds1Object.y;
        data.flags = (int)ds1Object.ds1_flags;

        foreach (var p in ds1Object.paths)
        {
            int x = (int)p.x;
            int y = (int)p.y;
            int action = (int)p.action;
            ObjectPathPoint pathPoint = new ObjectPathPoint(x, y, action);
            points.Add(pathPoint);
        }
    }
    private void UpdatePaths()
    {
        //For creating line renderer object
        var lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 1.0f;
            lineRenderer.endWidth = 1.0f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.loop = true;
            var material = new Material(Shader.Find("Standard"));
            lineRenderer.sharedMaterial = material;
            lineRenderer.sharedMaterial.color = Color.green;
        }

        lineRenderer.positionCount = points.Count + 1;
        // First point is object position itself
        var objPos = gameObject.transform.localPosition;
        lineRenderer.SetPosition(0, new Vector3(-objPos.x, objPos.y, objPos.z));

        int index = 1;
        foreach (var p in points) 
        {
            Vector3 pos = FromSubtileToPosition(-p.x, p.y);
            lineRenderer.SetPosition(index, pos);
            index++;
        }
    }

    private Vector3 FromSubtileToPosition(int subtileX, int subtileY)
    {
        var subtileStep = CoordinateSystem.SUBTILE_GRID_STEP;
        return new Vector3(subtileStep.x * subtileX, subtileStep.y, subtileStep.z * subtileY);
    }

    public DS1Object SerializeToObject()
    {
        DS1Object obj = new DS1Object();
        obj.type = data.type;
        obj.id = data.id;
        obj.x = data.x;
        obj.y = data.y;
        obj.ds1_flags = data.flags;

        obj.paths.Clear();

        for (int i = 0; i < points.Count; ++i)
        {
            DS1ObjectPath path = new DS1ObjectPath();
            path.x = points[i].x;
            path.y = points[i].y;
            path.action = points[i].action;
            obj.paths.Add(path);
        }

        return obj;
    }

}
