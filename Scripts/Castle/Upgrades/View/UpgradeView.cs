using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Upgrades;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private List<int> _levelCosts;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private UpgradeViewMessagesView _messageView;
    [SerializeField] private float _buyActualizeDelay;

    [Inject] private UpgradesController _upgradesController;
    [Inject] private ResourceController _resourceController;
    [Inject] private Player _player;
    
    private UpgradeModel _upgradeModel;
    private ItemType CostType => ItemType.Diamond;
    private IStack Stack => _player.Stack.SoftCurrencyStack;
    public int CurrentPrice => _levelCosts[Math.Min(_upgradeModel.CurrentLevel, _levelCosts.Count - 1)];

    public UnityAction Upgraded { get; set; }

    [Inject]
    private void OnInject()
    {
        _upgradeModel = _upgradesController.GetModel(_upgrade);
        Actualize();
        _player.Stack.SoftCurrencyStack.CountChanged += OnCountChanged;
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
    }

    private void OnCountChanged(int count)
    {
        Actualize();
    }
    
    public void Actualize()
    {
        if (_upgradeModel.CanLevelUp() == false)
        {
            _priceText.text = "MAX";
            _iconImage.gameObject.SetActive(false);
            return;
        }

        if (CurrentPrice == 0)
        {
            _priceText.text = "FREE";
            _iconImage.gameObject.SetActive(false);
            return;
        }
        _iconImage.gameObject.SetActive(true);
        _priceText.text = CurrentPrice.ToString();
        _iconImage.sprite = _resourceController.GetPrefab(CostType).Icon;
    }

    [NaughtyAttributes.Button("Click")]
    private void OnUpgradeButtonClick()
    {
        if(_upgradeModel.CanLevelUp() == false)
        {
            _messageView.Max();
            return;
        }

        if (HaveEnoughResource() == false)
        {
            _messageView.NotEnoughCoins();
            return;
        }
        
        if(Stack.TrySpend(CostType, CurrentPrice) == false)
            return;

        Upgrade();
    }

    private void Upgrade()
    {
        _upgradeModel.LevelUp();
        Upgraded?.Invoke();

        DOTween.Kill(this);
        DOVirtual.DelayedCall(_buyActualizeDelay, Actualize).SetId(this);
    }
    
    private bool HaveEnoughResource()
    {
        return Stack.Items[CostType].Value >= CurrentPrice;
    }
    
    
}
