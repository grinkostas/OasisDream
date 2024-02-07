using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Scripts.MarketLogic.Customers
{
    public class RequestView : MonoBehaviour
    {
        [SerializeField] private View _view;
        [SerializeField] private View _unavailableView;
        [SerializeField] private Image _requestIcon;
        [SerializeField] private TMP_Text _requestAmountText;
        [SerializeField] private Image _progressImage;
        
        [Inject, UsedImplicitly] public ResourceController ResourceController { get; }

        private float _progress = 0.0f;
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                _progressImage.fillAmount = _progress;
            }
        }
        
        public void ApplyRequest(Request request)
        {
            _requestIcon.sprite = ResourceController.GetPrefab(request.Type).Icon;
            _requestAmountText.text = request.Amount.ToString();
            _view.Show();
        }

        public void CompleteRequest()
        {
            _view.Hide();
            MakeAvailable();
            _progress = 0.0f;
        }

        public void MakeAvailable()
        {
            _unavailableView.Hide();
        }

        public void MakeUnavailable()
        {
            Progress = 0.0f;
            _unavailableView.Show();
        }
    }
}