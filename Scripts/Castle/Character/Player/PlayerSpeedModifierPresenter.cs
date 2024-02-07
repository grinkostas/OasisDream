using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class PlayerSpeedModifierPresenter : MonoBehaviour
{
    [SerializeField] private View _view;
    [Inject, UsedImplicitly] public Player Player { get; }

    private ModifiedValue<float> Speed => Player.CharacterControllerMovement.Speed;

    private void OnEnable()
    {
        Speed.Changed.On(Actualize);
    }

    private void OnDisable()
    {
        Speed.Changed.Off(Actualize);
    }

    private void Start()
    {
        Actualize();
    }

    private void Actualize()
    {
        if(Speed.Modified)
            _view.Show();
        else
            _view.Hide();
    }
}
