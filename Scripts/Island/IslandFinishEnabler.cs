using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class IslandFinishEnabler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _showOnFinish;
        [SerializeField] private List<GameObject> _hideOnFinish;

        [Header("Next Island Materials")] 
        [SerializeField] private Renderer _nextIslandModel;
        [SerializeField] private List<Material> _lockedMaterials;
        [SerializeField] private List<Material> _unlockedMaterials;
        [Inject] public Island Island { get; }

        private void OnEnable()
        {
            ActualizeObjectsActive();
            ActualizeMaterials();
            Island.Finished.Once(OnIslandFinished);
        }

        private void ActualizeObjectsActive()
        {
            foreach (var showOnFinish in _showOnFinish)
                showOnFinish.SetActive(Island.IsFinished);

            foreach (var hideOnFinish in _hideOnFinish)
                hideOnFinish.SetActive(Island.IsFinished == false);
        }

        private void ActualizeMaterials()
        {
            if (Island.IsFinished)
                SetMaterials(_unlockedMaterials);
            else
                SetMaterials(_lockedMaterials);
        }

        private void OnIslandFinished()
        {
            ActualizeObjectsActive();
            ActualizeMaterials();
        }

        private void SetMaterials(List<Material> materials)
        {
            _nextIslandModel.materials = materials.ToArray();
        }
    }
}