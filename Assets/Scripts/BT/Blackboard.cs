using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }
    // public void Set<T>(string key, T value) => _data[key] = value;

    public T Get<T>(string key)
        => _data.TryGetValue(key, out var o) && o is T t ? t : default;

    public bool HasKey(string key) => _data.ContainsKey(key);

    public void Remove(string key) => _data.Remove(key);
}