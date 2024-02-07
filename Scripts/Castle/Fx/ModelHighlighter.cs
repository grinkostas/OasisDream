using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ModelHighlighter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlightParticle;
    [SerializeField] private Transform _modelToHighlight;
    [SerializeField] private Material _highlightedMaterial;
    [SerializeField] private float _duration;
    [SerializeField] private bool _includeInactiveModels;

    private Dictionary<Renderer, List<Material>> _defaultMaterialsData = new();
    private List<Renderer> _characterModels = new();
    
    private void Awake()
    {
        GetDefaultMaterials();
    }
    
    private void GetDefaultMaterials()
    {
        _defaultMaterialsData.Clear();
        _characterModels = _modelToHighlight.GetComponentsInChildren<Renderer>(_includeInactiveModels).ToList();
        foreach (var model in _characterModels)
        {
            var modelMaterials = new List<Material>(model.materials);
            _defaultMaterialsData.Add(model, modelMaterials);
        }
    }

    public void Highlight()
    {
        PlayParticle();
        SetHighlightedMaterial();
        DOVirtual.DelayedCall(_duration, SetDefaultMaterial);
    }
    
    private void PlayParticle()
    {
        if(_highlightParticle == null)
            return;
        
        _highlightParticle.Stop();
        _highlightParticle.time = 0;
        _highlightParticle.Play();
    }
    
    private void SetHighlightedMaterial()
    {
        foreach (var model in _characterModels)
        {
            model.materials = new[] { _highlightedMaterial };
        }
    }
    
    private void SetDefaultMaterial()
    {
        foreach (var defaultMaterialData in _defaultMaterialsData)
        {
            defaultMaterialData.Key.materials = defaultMaterialData.Value.ToArray();
        }
    }

}
