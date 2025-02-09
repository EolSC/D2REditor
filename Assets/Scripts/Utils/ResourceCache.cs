using System.Collections.Generic;
using UnityEngine;

public class ResourceCache
{
    private Dictionary<string, object> storage = new Dictionary<string, object>();


    public void AddResource(string name, object resource)
    {
        storage[name] = resource;
    }

    public bool Get(string name, ref object obj)
    {
        if (storage.ContainsKey(name))
        {
            obj = storage[name];
            return true;
        }
        return false;
    }

    public void Clear()
    {
        storage.Clear();
    }
}
