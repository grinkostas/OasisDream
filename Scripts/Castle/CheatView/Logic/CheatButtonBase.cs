using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class CheatButtonBase : MonoBehaviour
{
    [SerializeField] private bool _externalCall = false;
    
    private Button _button;
    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();
            return _button;
        }
    }

    private void OnEnable()
    {
        if(_externalCall)
            return;
        Button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        if(_externalCall)
            return;
        Button.onClick.RemoveListener(OnButtonClicked);
    }

    public abstract void OnButtonClicked();
}
