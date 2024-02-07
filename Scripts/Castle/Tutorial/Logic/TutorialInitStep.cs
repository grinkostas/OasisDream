using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.UI;

public class TutorialInitStep : MonoBehaviour
{
    [SerializeField] private TutorialStepBase _targetStep;
    [SerializeField] private bool _onEnter;
    [SerializeField] private bool _onExit;
    [SerializeField] private List<GameObject> _objectToActivate;
    [SerializeField] private List<GameObject> _objectToDeactivate;
    [SerializeField] private List<Button> _buttonsToDisable;
    [SerializeField] private List<Button> _buttonsToEnable;

    private bool _ended = false;
    
    private void Awake()
    {
        if (_targetStep.IsFinished)
        {
            if (_onExit)
            {
                OnAction(_targetStep);
            }
            return;
        }
        ChangeActive(_objectToActivate, false);
    }

    private void OnEnable()
    {
        if(_onEnter)
            _targetStep.Entered += OnAction;
        if(_onExit)
            _targetStep.Ended += OnAction;
    }

    private void OnDisable()
    {
        End();
    }


    private void OnAction(TutorialStepBase tutorialStep)
    {
        End();
    }

    public void End()
    {
        if(_ended)
            return;
        _ended = true;
        ChangeActive(_objectToActivate, true);
        ChangeActive(_objectToDeactivate, false);

        ChangeInteractable(_buttonsToEnable, true);
        ChangeInteractable(_buttonsToDisable, false);
    }

    private void ChangeActive(List<GameObject> list, bool active)
    {
        foreach (var obj in list)
        {
            obj.SetActive(active);
        }
    }

    private void ChangeInteractable(List<Button> buttons, bool active)
    {
        foreach (var button  in buttons)
        {
            button.interactable = active;
        }
    }

}
