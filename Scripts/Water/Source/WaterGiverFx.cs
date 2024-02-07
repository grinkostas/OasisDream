using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class WaterGiverFx : MonoBehaviour
    {
        [SerializeField] private WaterGiver _waterGiver;
        [SerializeField] private float _sourceRadius;
        [Header("WaterGun")] 
        [SerializeField] private Vector3 _waterGunStartAngles;
        [SerializeField] private Vector3 _waterGunRotateAngles;
        [SerializeField] private float _waterGunRotationDuration;

        [Header("WaterBottle")] 
        [SerializeField] private float _waterBottlePunch;
        [SerializeField] private float _waterBottlePunchDuration;
        [Header("PlayerRotation")] 
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private float _lookAtRotationDuration;
        [Space] 
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private string _holdHoseParameter = "HoldPipeExternal";

        private bool _receivingWater = false;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _sourceRadius);
        }

        private void OnEnable()
        {
            _waterGiver.StartedReceiving.On(OnStartedReceiving);
            _waterGiver.StoppedReceiving.On(OnStoppedReceiving);
        }

        private void OnDisable()
        {
            _waterGiver.StartedReceiving.Off(OnStartedReceiving);
            _waterGiver.StoppedReceiving.Off(OnStoppedReceiving);
        }

        private void OnStartedReceiving(WaterBottle waterBottle)
        {
            DOTween.Kill(this);
            _receivingWater = true;
            _particle.transform.position =
                VectorExtentions.GetPointOnCircle(waterBottle.transform.position, transform.position, _sourceRadius);
            _particle.Play();
            _waterGiver.Player.Animator.SetBool(_holdHoseParameter, true);
            var model = waterBottle.WaterBottleModel;
            PunchBottle(model);
            model.WaterGunWithHose.gameObject.SetActive(true);
            model.Holder.DOLookAt(_lookAtPoint.position, _lookAtRotationDuration, AxisConstraint.Y).SetId(this);
            model.WaterGun.DOLocalRotate(_waterGunRotateAngles, _waterGunRotationDuration).SetId(this);
            model.Hose.ReceiveWater();
        }

        private void OnStoppedReceiving(WaterBottle waterBottle)
        {
            DOTween.Kill(this);
            _receivingWater = false;
            _particle.Stop();
            _waterGiver.Player.Animator.SetBool(_holdHoseParameter, false);
            var model = waterBottle.WaterBottleModel;
            model.WaterGunWithHose.gameObject.SetActive(false);
            model.WaterGun.DOLocalRotate(_waterGunStartAngles, _waterGunRotationDuration).SetId(this);
            model.Hose.ResetBlendShape();
        }

        private void PunchBottle(WaterBottleModel model)
        {
            var sequence = DOTween.Sequence();
            sequence.Append( model.BarrelWrapper.DOPunchScale(Vector3.one * _waterBottlePunch, _waterBottlePunchDuration, 2));
            sequence.Join( model.WaterGun.DOPunchScale(Vector3.one * _waterBottlePunch, _waterBottlePunchDuration, 2));
            sequence.SetId(_waterBottlePunch);
            sequence .OnComplete(() =>
                {
                    if (_receivingWater)
                        PunchBottle(model);
                });
        }
    }
}