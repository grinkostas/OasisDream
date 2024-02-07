using DG.Tweening;
using GameCore.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Scripts.Tasks
{
    public class TaskBarView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _taskDescription;
        [SerializeField] private SimpleSlider _progressSlider;
        [SerializeField] private TextSynchronizer _progressText;
        [Header("Animation")] 
        [SerializeField] private GameObject _taskBarIconParent;
        [SerializeField] private CanvasGroup _checkMark;
        [SerializeField] private CanvasGroup _contentGroup;
        [SerializeField] private float _animationDuration;
        
        private ITask _currentTask;
        public bool Hidden { get; private set; } = true;

        private void Awake()
        {
            Prepare(_checkMark, 0);
            Prepare(_contentGroup, 0, true);
        }
        
        public void InitTaskView(ITask task)
        {
            if(task == null)
                return;

            if (_currentTask != null)
                _currentTask.CurrentValueChanged.Off(ActualizeProgress);

            _currentTask = task;
            _icon.sprite = task.Icon;
            _taskDescription.text = task.Description;
            task.CurrentValueChanged.On(ActualizeProgress);
            ActualizeProgress(task.CurrentValue);
        }

        public void ActualizeProgress(int currentValue)
        {
            _progressSlider.Value = currentValue/(float)_currentTask.FinishValue;
            _progressText.text = $"{currentValue}/{_currentTask.FinishValue}";
        }

        public void Show()
        {
            if(Hidden == false)
                return;
            
            DOTween.Kill(this);
            Hidden = false;
            Prepare(_contentGroup, 0, true);
            Prepare(_checkMark, 0, false);
            _taskBarIconParent.gameObject.SetActive(true);
            Animate(_contentGroup, 1);
        }

        public void Complete()
        {
            DOTween.Kill(this);
            Prepare(_checkMark, 0);
            Animate(_checkMark, 1).OnComplete(()=>_taskBarIconParent.gameObject.SetActive(false));
        }

        public void Hide()
        {
            if(Hidden)
                return;
            DOTween.Kill(this);
            Hidden = true;
            Animate(_contentGroup, 0, Ease.InBack);
        }
        
        private void Prepare(CanvasGroup canvasGroup, float startValue, bool active = true)
        {
            if(canvasGroup == null)
                return;
            canvasGroup.alpha = startValue;
            canvasGroup.transform.localScale = Vector3.one * startValue;
            canvasGroup.gameObject.SetActive(active);
        }
        
        private Tween Animate(CanvasGroup canvasGroup, float endValue, Ease zoomEase = Ease.OutBack)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.transform.DOScale(endValue, _animationDuration).SetEase(zoomEase));
            sequence.Join(canvasGroup.DOFade(endValue, _animationDuration));
            sequence.SetId(this);
            return sequence;
        }

        
        
    }
}