using DG.Tweening;
using MPUIKIT;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;


public class SimpleSlider : MonoBehaviour
{
    [SerializeField] private MPImage _fillImage;
    [SerializeField, ShowIf(nameof(_imageCheck))] private float _value;
    [SerializeField] private float _minValue = 1;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _actualizeDuration;
    private bool _imageCheck => _fillImage != null;
    public float Value
    {
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, _minValue, _maxValue);
            DOTween.Kill(this);
            DOVirtual.Float(_fillImage.fillAmount, _value, _actualizeDuration,
                fillAmount => _fillImage.fillAmount = fillAmount).SetEase(Ease.Linear).SetId(this);
            ProgressChanged.Dispatch(_value);
        }
    }

    public TheSignal<float> ProgressChanged { get; } = new();

    private void OnValidate()
    {
        if(_fillImage == null)
            return;
        _fillImage.fillAmount = _value;
    }
}
