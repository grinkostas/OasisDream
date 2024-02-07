using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using StaserSDK.Stack;
using UnityEngine.SceneManagement;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;

    [Inject] private ResourceController _resourceController;
    [Inject] private Player _player;
    
    [Button("Reset Saves")]
    public void ResetSaves()
    {
        ES3.DeleteFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Button("Add10Resource")]
    private void Add100Resource()
    {
        for (int i = 0; i < 10; i++)
        {
            var item = _resourceController.GetInstance(_itemType);
            if (_itemType == ItemType.Diamond)
                _player.Stack.SoftCurrencyStack.TryAddRange(_itemType, 10);
            else if(_player.Stack.MainStack.TryAdd(item) == false)
                break;
        }
        

    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Application.targetFrameRate = 90;
    }
}
