using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public class HelperLockActivator : MonoBehaviour
{
    [SerializeField] private HelperLocker _helperLocker;
    [SerializeField] private List<GameObject> _objectsToActivate;
    [SerializeField] private List<GameObject> _objectsToDeactivate;

    private void OnEnable()
    {
        Init();
        if (_helperLocker.IsLocked)
        {
            OnRelease();
            return;
        }

        _helperLocker.Released += OnRelease;
    }

    private void OnDisable()
    {
        _helperLocker.Released -= OnRelease;
    }

    private void Init()
    {
        _objectsToActivate.ChangeActive(false);
        _objectsToDeactivate.ChangeActive(true);
    }
    
    private void OnRelease()
    {
        _objectsToActivate.ChangeActive(true);
        _objectsToDeactivate.ChangeActive(false);
    }
}
