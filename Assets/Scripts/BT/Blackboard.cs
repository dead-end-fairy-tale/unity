using System;
using System.Collections.Generic;

public class Blackboard
{
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
    public event Action<string, object> OnValueChanged;

    public void Set<T>(string key, T value)
    {
        if (_data.TryGetValue(key, out var existingObj) &&
            existingObj is T existingValue &&
            EqualityComparer<T>.Default.Equals(existingValue, value))
        {
            return;
        }
        
        _data[key] = value;
        OnValueChanged?.Invoke(key, value);
    }

    public T Get<T>(string key)
        => _data.TryGetValue(key, out var o) && o is T t ? t : default;

    public bool HasKey(string key) => _data.ContainsKey(key);

    public void Remove(string key) => _data.Remove(key);
}