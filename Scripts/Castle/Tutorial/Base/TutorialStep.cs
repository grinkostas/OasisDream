using UnityEngine;

public class TutorialStep : TutorialStepBase
{
    [SerializeField] private Transform _targetPoint;
    public override Transform Target => _targetPoint;
}
