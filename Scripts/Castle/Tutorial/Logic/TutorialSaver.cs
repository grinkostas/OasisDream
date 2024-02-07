using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixSignals;

public class TutorialSaver : MonoBehaviour
{
    [SerializeField] private TutorialStepBase _lastStep;
    [SerializeField] private string _tutorialEndId;

    public bool Completed => ES3.Load(_tutorialEndId, false);
    public TheSignal OnCompleted { get; } = new();
    private void OnEnable()
    {
        _lastStep.Ended += OnExit;
    }

    private void OnExit(TutorialStepBase step)
    {
        ES3.Save(_tutorialEndId, true);
        OnCompleted.Dispatch();
    }
}
