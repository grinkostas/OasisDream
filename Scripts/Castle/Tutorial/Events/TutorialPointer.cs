using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TutorialPointer : MonoBehaviour
{
    [SerializeField] private GameObject _verticalPointer;
    
    [SerializeField] private GameObject _playerHorizontalPointer;
    [SerializeField] private Vector3 _lookAxis;
    [SerializeField] private float _minShowDistance;

    [Inject] private Player _player;
    
    private bool _hidden = true;
    private Transform _target;
    

    private void Awake()
    {
        HidePointers();
    }

    public void SetTarget(Transform target)
    {
        _hidden = false;
        _target = target;
        _verticalPointer.transform.SetParent(target);
        _verticalPointer.transform.localPosition = Vector3.zero;
        _verticalPointer.SetActive(true);
    }

    public void ReceiveDestination()
    {
        _hidden = true;
        _target = null;
        HidePointers();
    }

    private void Update()
    {
        if(_hidden)
            return;

        Transform pointerModel = _playerHorizontalPointer.transform;
        
        pointerModel.LookAt(_target);
        pointerModel.rotation = Quaternion.Euler(Vector3.Scale(pointerModel.rotation.eulerAngles, _lookAxis));
        pointerModel.gameObject.SetActive(GetDistance(_target) >= _minShowDistance*_minShowDistance);
    }

    private void HidePointers()
    {
        _verticalPointer.SetActive(false);
        _playerHorizontalPointer.SetActive(false);
    }
    
    private float GetDistance(Transform target)
    {
        return Vector3.Distance(_playerHorizontalPointer.transform.position.XZ(), target.position.XZ());
    }
}
