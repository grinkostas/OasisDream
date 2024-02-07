using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public class MushroomModifierDisplay : ModifierDisplay
    {
        [SerializeField] private float _scale;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private List<Renderer> _attachedRenderers;
        [SerializeField] private Material _modifierMaterial;
        [SerializeField] private float _flashDelay;
        [Inject] public Player Player { get; }

        private List<STuple<Renderer, List<Material>>> _startMaterials = new();

        protected override void OnAppliedModifier()
        {
            DOTween.Kill(this);
            Player.transform.DOScale(_scale, _scaleDuration).SetEase(Ease.OutBack).SetId(this);
            GetStartMaterials();
            SetHighlightMaterials();
        }

        private void GetStartMaterials()
        {
            foreach (var attachedRenderer in _attachedRenderers)
            {
                _startMaterials.Add(new(attachedRenderer, attachedRenderer.materials.ToList()));
            }
        }

        private void SetDefaultMaterials()
        {
            foreach (var startMaterial in _startMaterials)
            {
                startMaterial.Value1.materials = startMaterial.Value2.ToArray();
            }
        }
        
        
        private void SetHighlightMaterials()
        {
            foreach (var attachedRenderer in _attachedRenderers)
            {
                var materials = attachedRenderer.materials.ToList();
                materials.Add(_modifierMaterial);
                attachedRenderer.materials = materials.ToArray();
            }

            DOVirtual.DelayedCall(_flashDelay, ResetHighlightMaterial).SetId(this);
        }

        private void ResetHighlightMaterial()
        {
            SetDefaultMaterials();
            DOVirtual.DelayedCall(_flashDelay, SetHighlightMaterials).SetId(this);
        }

        protected override void OnAppliedDisappear()
        {
            DOTween.Kill(this);
            Player.transform.DOScale(1, _scaleDuration).SetEase(Ease.InBack).SetId(this);
            SetDefaultMaterials();
        }
    }
}