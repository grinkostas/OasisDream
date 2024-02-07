using DG.Tweening;
using UnityEngine;


public class DamagePunch : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private Vector3 _punchRotationAxis;
    [SerializeField] private Vector2 _rotationRange;
    [SerializeField] private float _punchDuration;

    private void OnEnable()
    {
        _destructible.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _destructible.HealthChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged()
    {
        if(DOTween.IsTweening(this))
            return;
        var punch = Vector3.Scale(_punchRotationAxis,GetRandomVector());
        transform.DOPunchRotation(punch, _punchDuration, 2).SetId(this);
    }

    private Vector3 GetRandomVector()
    {
        return new Vector3(RandomNext(), RandomNext(), RandomNext());
    }
    
    private float RandomNext()
    {
        int minus = Random.Range(0, 2) == 0 ? 1 : -1;
        return _rotationRange.Random() * minus;
    }
}
