using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    private List<T> objects = new List<T>();

    public void Initialize()
    {
        objects.Clear();
    }

    public T Get(int index)
    {
        return objects[index] ?? objects[0];
    }

    public void Add(T obj)
    {
        if (!objects.Contains(obj)) {
            objects.Add(obj);
        }
    }

    public void Remove(T obj)
    {
        if (objects.Contains(obj)) {
            objects.Remove(obj);
        }
    }
}
