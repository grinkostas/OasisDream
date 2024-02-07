using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Popups
{
    public class PopupFactory
    {
        [Inject, UsedImplicitly] public CanvasController CanvasController { get; }
        [Inject, UsedImplicitly] public DiContainer Container { get; }
        
        private List<PopupData> _spawnedPopups = new();
        public PopupSettings PopupSettings => PopupSettings.Value;

        public TheSignal<APopup> Showed { get; } = new();
        public TheSignal<APopup> Hided { get; } = new();

        public TPopup Get<TPopup>() where TPopup : APopup
        {
            if (IsAlreadySpawned<TPopup>())
                return (TPopup)_spawnedPopups.Find(x => x.Popup.Type == typeof(TPopup)).Popup;

            return SpawnPopup<TPopup>(PopupSettings.GetData<TPopup>());
        }

        public APopup Get(string id)
        {
            if (IsAlreadySpawned(id))
                return _spawnedPopups.Find(x => x.Id == id).Popup;
            
            var popup = SpawnPopup<APopup>(PopupSettings.GetData(id));
            var castedPopup = System.Convert.ChangeType(popup, popup.Type);
            return popup;
        }

        private TPopup SpawnPopup<TPopup>(PopupData popupData) where TPopup : APopup
        {
            if (popupData == null)
                throw new System.Exception($"Cannot find popup of type {typeof(TPopup)}");
            
            var popup = (TPopup)Object.Instantiate(popupData.Popup, CanvasController.Get(popupData.Popup.CanvasType).transform);
            Container.InjectGameObject(popup.gameObject);

            _spawnedPopups.Add(new PopupData(popupData.Id, popup));
            return popup;
        }

        public void Show<TPopup>(float delay = 0.0f) where TPopup : APopup
        {
            var popup = Get<TPopup>();
            popup.Show(delay);
            Showed.Dispatch(popup);
        }
        
        public void Show(string id, float delay = 0.0f)
        {
            var popup = Get(id);
            popup.Show(delay);
            Hided.Dispatch(popup);
        }
        
        public void Hide<TPopup>(float delay = 0.0f) where TPopup : APopup
        {
            if (IsAlreadySpawned<TPopup>() == false)
                return;

            var popup = Get<TPopup>();
            popup.Hide(delay);
        }

        public bool IsAlreadySpawned<TPopup>()
        {
            return _spawnedPopups.Has(x => x.Popup.Type == typeof(TPopup));
        }
        
        public bool IsAlreadySpawned(string id)
        {
            return _spawnedPopups.Has(x => x.Id ==  id);
        }
    }
}