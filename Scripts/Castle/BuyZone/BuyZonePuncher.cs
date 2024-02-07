using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using StaserSDK.Stack;
using Zenject;

public class BuyZonePuncher : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private StackSize _obstacleSize;
    [SerializeField] private float _moveTime;

    [Inject] private Player _player;
    [Inject] private InputHandler _inputHandler;
    private Vector3 PlayerPosition => _player.transform.position;
    private Vector3 BottomLeftCorner => _obstacleSize.transform.position + _obstacleSize.Center - _obstacleSize.Size/2;
    private Vector3 TopRightCorner =>  _obstacleSize.transform.position + _obstacleSize.Center + _obstacleSize.Size/2;
    
    private void OnEnable()
    {
        _buyZone.Bought.On(OnBought);
    }
    
    private void OnDisable()
    {
        _buyZone.Bought.Off(OnBought);
    }

    private void OnBought()
    {
        if(IsLocatedInside() == false)
            return;
        Punch();

    }
    
    private bool IsLocatedInside()
    {
        if (PlayerPosition.x > BottomLeftCorner.x && PlayerPosition.x < TopRightCorner.x &&
            PlayerPosition.z > BottomLeftCorner.z && PlayerPosition.z < TopRightCorner.z)
            return true;
        
        return false;
    }

    private void Punch()
    {
        _inputHandler.DisableHandle(this);
        
        float xDistance = StaserMath.MinAbs(PlayerPosition.x - BottomLeftCorner.x, PlayerPosition.x - TopRightCorner.x);
        float zDistance = StaserMath.MinAbs(PlayerPosition.z - TopRightCorner.z, PlayerPosition.z - BottomLeftCorner.z);
        
        Vector3 playerDestination = PlayerPosition;

        if (Mathf.Abs(xDistance) < Mathf.Abs(zDistance))
        {
            playerDestination.x = xDistance > 0 ? BottomLeftCorner.x : TopRightCorner.x;
        }
        else
        {
            playerDestination.z = zDistance > 0 ? TopRightCorner.z : BottomLeftCorner.z;
        }

        _player.transform.DOMove(playerDestination, _moveTime).SetEase(Ease.OutCubic)
            .OnComplete(() => _inputHandler.EnableHandle(this));
    }
    
    
}
