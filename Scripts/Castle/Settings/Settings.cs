using UnityEngine;

public abstract class Settings<T> : ScriptableObject where T : ScriptableObject
{
    protected static T _cachedDefault;

    public static T Value
    {
        get
        {
            if (_cachedDefault == null)
            {
                _cachedDefault = Resources.Load<T>($"Settings/{typeof(T).Name}");
            }
            return _cachedDefault;
        }
    }
}
