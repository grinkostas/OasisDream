using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializedProperty<T>
{
    private T _value;
    private bool _initialized = false;
    private Func<T> _constructor;
    private bool _isConst;
    
    public T Value
    {
        get
        {
            Init();
            return _value;
        }
        set
        {
            if (_isConst)
                throw new Exception("Tried to change const value");
            _value = value;
        }
    }

    public void Init()
    {
        if (_initialized == false)
            _value = _constructor();
        _initialized = true;
    }

    public InitializedProperty(Func<T> constructor, bool isConst = false)
    {
        _constructor = constructor;
        _isConst = isConst;
    }

}
