using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TutorialPointerInstaller : MonoInstaller
{
    [SerializeField] private TutorialPointer _tutorialPointer;
    public override void InstallBindings()
    {
        Container.Bind<TutorialPointer>().FromInstance(_tutorialPointer);
    }
}
