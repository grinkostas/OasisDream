using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Scripts.Utilities
{
    public class ImageLinker : MonoBehaviour
    {
        [SerializeField] private Sprite _linkedSprite;
        [SerializeField] private List<Image> _imagesList;

        private void Awake()
        {
            Change();
        }

        [Button()]
        private void Change()
        {
            foreach (var image in _imagesList)
            {
                image.sprite = _linkedSprite;
            }
        }
    }
}