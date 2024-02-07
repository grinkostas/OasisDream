using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core.Easing;
using StaserSDK.Stack;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class BuyZoneCostView : MonoBehaviour
{
   [SerializeField] private Image _image;
   [SerializeField] private TMP_Text _progressText;
   [SerializeField] private ZoomView _zoomView;
   [SerializeField] private Bouncer _bouncer;

   [Inject] private ResourceController _resourceController;
   
   private BuyZone _buyZone;
   private ItemType _itemType;
   
   private int _currentBuyProgress = 0;

   public ItemType ItemType => _itemType;

  

   public void Init(BuyZone buyZone, ItemType type)
   {
      _buyZone = buyZone;
      _itemType = type;
      _image.sprite = _resourceController.GetPrefab(type).Icon;
      Init();
   }
   
   private void Init()
   {
      if (_buyZone.SourcePrices.ContainsKey(_itemType) == false)
      {
         _zoomView.Hide();
         return;
      }
      Actualize();
   }
   public void Actualize()
   {
      int currentProgress = _buyZone.Prices[_itemType];
      Actualize(currentProgress);
   }

   public void Actualize(int progress)
   {
      if (progress <= 0)
      {
         gameObject.SetActive(false);
         return;
      }
      if (_currentBuyProgress != progress)
      {
         _progressText.text = progress.ToString();
         _currentBuyProgress = progress;
      }
      _bouncer.Bounce();
   }


}
