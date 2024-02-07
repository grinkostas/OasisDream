using JetBrains.Annotations;
using NaughtyAttributes;
using StaserSDK.Interactable;
using StaserSDK.Views;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Popups
{
    public class PopupZonePresenter : MonoBehaviour
    {
        [SerializeField] private string _popupId;
        [SerializeField] private ZoneBase _zoneBase;
        [SerializeField] private PresenterAction _enterAction;
        [SerializeField] private PresenterAction _exitAction;
        [SerializeField] private PresenterAction _interactAction;

        [Inject, UsedImplicitly] public PopupFactory PopupFactory { get; }

        private void OnEnable()
        {
            _zoneBase.OnEnter += OnEnter;
            _zoneBase.OnExit += OnExit;
            _zoneBase.OnInteract += OnInteract;
        }
        
        private void OnDisable()
        {
            _zoneBase.OnEnter -= OnEnter;
            _zoneBase.OnExit -= OnExit;
            _zoneBase.OnInteract -= OnInteract;
        }

        private void OnEnter(InteractableCharacter character)
        {
            HandlePresenterAction(_enterAction);
        }
        
        private void OnExit(InteractableCharacter character)
        {
            HandlePresenterAction(_exitAction);
        }
        
        private void OnInteract(InteractableCharacter character)
        {
            HandlePresenterAction(_interactAction);
        }
        
        private void HandlePresenterAction(PresenterAction enterAction)
        {
            switch (enterAction)
            {
                case PresenterAction.Hide:
                    PopupFactory.Get(_popupId).Hide();
                    break;
                case PresenterAction.Show:
                    PopupFactory.Get(_popupId).Show();
                    break;
            }
        }

        [Button()]
        private void Show()
        {
            PopupFactory.Get(_popupId).Show();
        }
    }
}