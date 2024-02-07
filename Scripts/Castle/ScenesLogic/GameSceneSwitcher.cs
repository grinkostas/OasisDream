using System;
using UnityEngine;


public class GameSceneSwitcher : MonoBehaviour
{
    private void Awake()
    {
        CastleScenes.LoadCurrentGameScene();
    }
}
