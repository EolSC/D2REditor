using System.Collections.Generic;
using UnityEngine;

public class DT1Cache
{
    private Dictionary<string, DT1Data> storage = new Dictionary<string, DT1Data>();


    public void Add(DT1Data data)
    {
        storage[data.fileName] = data;
    }

    public bool Get(string name, ref DT1Data data)
    {
        if (storage.ContainsKey(name))
        {
            data = storage[name];
            return true;
        }
        return false;
    }

    public void Clear()
    {
        storage.Clear();
    }
}
