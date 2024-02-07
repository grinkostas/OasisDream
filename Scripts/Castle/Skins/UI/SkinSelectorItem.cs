using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class SkinSelectorItem : MonoBehaviour, IPoolItem<SkinSelectorItem>
{
    [SerializeField] private Image _skinIconImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _selectButton;
    [SerializeField] private bool _haveLockImage;
    [SerializeField] private bool _haveCheckMarkImage;
    [SerializeField, ShowIf(nameof(_haveLockImage))] private Image _lockImage;
    [SerializeField, ShowIf(nameof(_haveCheckMarkImage))] private Image _checkMartImage;

    [Inject] private SkinManager _skinManager;
    public IPool<SkinSelectorItem> Pool { get; set; }
    public bool IsTook { get; set; }

    public bool Initialized { get; private set; }
    public SkinData SkinData { get; private set; }

    public Button Button => _selectButton;

    public event UnityAction<SkinSelectorItem> Selected;
    
    public SkinSelectorItem Init(SkinData skinData)
    {
        SkinData = skinData;
        Initialized = true;
        _selectButton.onClick.AddListener(OnSelect);
        Actualize();
        return this;
    }

    private void Actualize()
    {
        _skinIconImage.sprite = SkinData.Icon;
        _backgroundImage.color = _skinManager.GetColor(SkinData.Rarity);

        ShowAdditionalGraphic();
    }

    private void ShowAdditionalGraphic()
    {
        if(_haveLockImage == false)
            return;

        if (SkinData.Available == false)
        {
            _lockImage.gameObject.SetActive(true);
            if(_haveCheckMarkImage)
                _checkMartImage.gameObject.SetActive(false);
            return;
        }

        _lockImage.gameObject.SetActive(false);
        if(_haveCheckMarkImage)
            _checkMartImage.gameObject.SetActive(true);
    }
    
    public void TakeItem()
    {
    }

    public void ReturnItem()
    {
        _selectButton.onClick.RemoveListener(OnSelect);
    }

    private void OnSelect()
    {
        Selected?.Invoke(this);
    }
    
}
