using System;
using StaserSDK.Interactable;
using UnityEngine;

namespace GameCore.Scripts.Castle.ScenesLogic
{
    public class TargetGameSceneInteractChanger : MonoBehaviour
    {
        [SerializeField] private ZoneBase _zoneBase;
        [SerializeField] private string _targetSceneName;
        private void OnEnable()
        {
            _zoneBase.OnInteract += OnInteract;
        }

        private void OnDisable()
        {
            _zoneBase.OnInteract -= OnInteract;
        }

        private void OnInteract(InteractableCharacter _)
        {
            CastleScenes.ChangeCurrentGameScene(_targetSceneName);
        }
    }
}