using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Stack;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player);
    }
}
