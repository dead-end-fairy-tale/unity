using UnityEngine;

public interface IAccountManager
{
    public bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
