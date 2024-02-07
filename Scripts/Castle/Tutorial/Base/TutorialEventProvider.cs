using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TutorialEventProvider : InterfaceProvider<ITutorialEvent>
{
    [RequireInterface(typeof(ITutorialEvent))] public GameObject _gameObject;
    protected override GameObject ObjectWithInterface => _gameObject;
}
