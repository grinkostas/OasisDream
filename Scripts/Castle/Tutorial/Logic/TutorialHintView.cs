using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TutorialHintView : MonoBehaviour
{
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private Image _hintIcon;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private View _view;
    [Header("Punch")]
    [SerializeField] private Transform _wrapper;
    [SerializeField] private float _punch;
    [SerializeField] private float _punchDuration;
    [Header("Hints")]
    [SerializeField] private List<TutorialHintData> _hintData;
    
    [Serializable]
    internal class TutorialHintData
    {
        public TutorialStepBase TutorialStep;
        public Sprite Sprite;
        public string Text;
    }

    private Tweener _punchTweener;
    private TutorialStepBase _currentTutorialStep;

    private void OnEnable()
    {
        _view.Hide();
        foreach (var hintData in _hintData)
        {
            hintData.TutorialStep.Entered += OnTutorialStepEntered;
            hintData.TutorialStep.Exited += OnTutorialStepExited;
        }
    }
    
    private void OnDisable()
    {
        foreach (var hintData in _hintData)
        {
            hintData.TutorialStep.Entered -= OnTutorialStepEntered;
            hintData.TutorialStep.Exited -= OnTutorialStepExited;
        }
    }

    private void OnTutorialStepEntered(TutorialStepBase step)
    {
        if(step.Task.IsFinished())
            return;
        _currentTutorialStep = step;
        var data = _hintData.Find(x => x.TutorialStep == step);
        _hintText.text = data.Text;
        _hintIcon.sprite = data.Sprite;
        _view.Show();
        float progress = step.Task.Progress;
        _progressSlider.value = progress;
        _progressText.text = $"{(int)(Mathf.Min(step.Task.FinishValue * progress, step.Task.FinishValue))}/{step.Task.FinishValue}";
        //step.Task.ProgressChanged += OnProgressChanged;
    }

    private void OnProgressChanged(float progress)
    {
        _progressSlider.value = progress;
        //_progressText.text = $"{(int)Mathf.Min(_currentTutorialStep.Task.FinishValue * progress, _currentTutorialStep.Task.FinishValue)}/{(int)_currentTutorialStep.Task.FinalValue}";
    }
    
    private void OnTutorialStepExited(TutorialStepBase step)
    {
        //step.Task.ProgressChanged -= OnProgressChanged;
        if(_punchTweener is { active: true })
            return;
        _punchTweener = _wrapper.DOPunchScale(_punch * Vector3.one, _punchDuration, 2);
        _view.Hide();
    }
}
