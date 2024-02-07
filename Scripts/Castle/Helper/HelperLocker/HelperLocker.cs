using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class HelperLocker : MonoBehaviour
{
    [SerializeField] private ResourcePlace _resourcePlace;
    [SerializeField] private Helper _helper;
    [SerializeField] private string _helperId;

    public Helper Helper => _helper;
    public bool IsLocked => ES3.Load(_helperId, false);
    
    public UnityAction Released { get; set; }
    private void OnEnable()
    {
        if(IsLocked)
            return;
        _helper.Handler.DisableHandle(this);
        _helper.Handler.enabled = false;
        _helper.Handler.Agent.SourceAgent.enabled = false;
        _resourcePlace.Finished += Release;
    }

    private void OnDisable()
    {
        _resourcePlace.Finished -= Release;
    }

    private void Start()
    {
        if (IsLocked)
            Release();
    }
    
    private void Release()
    {
        ES3.Save(_helperId, true);
        _helper.Handler.Agent.SourceAgent.enabled = true;
        _helper.Handler.enabled = true;
        _helper.Handler.EnableHandle(this);
        Released?.Invoke();
    }
}
