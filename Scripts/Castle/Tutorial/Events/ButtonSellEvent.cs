using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSellEvent : MonoBehaviour, ITutorialEvent
{
    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    public UnityAction<float> ProgressChanged { get; set; }
    public float Progress { get; }
    public float FinalValue { get; }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public bool IsFinished()
    {
        throw new System.NotImplementedException();
    }

    public bool IsAvailable()
    {
        throw new System.NotImplementedException();
    }
}
