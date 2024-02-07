using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Zenject;

[RequireComponent(typeof(Recycler))]
public class RecycleBinder : MonoBehaviour
{
    [SerializeField] private string _id;

    [Inject, UsedImplicitly] public RecyclersController RecyclersController { get; }
    
    private InitializedProperty<Recycler> _recyclerProperty;
    private Recycler _recycler;

    [Inject]
    public void OnInject()
    {
        _recyclerProperty = new InitializedProperty<Recycler>(() => gameObject.GetComponent<Recycler>());
        RecyclersController.Add(_id, _recyclerProperty.Value);
    }
    
}
