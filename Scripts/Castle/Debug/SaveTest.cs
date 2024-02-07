using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ES3Internal;
using NaughtyAttributes;
using StaserSDK.Stack;
using Debug = System.Diagnostics.Debug;

public class SaveTest : MonoBehaviour
{
    private Dictionary<ItemType, int> _testDictionary = new Dictionary<ItemType, int>
    {
        { ItemType.Beam, 128 },
        { ItemType.Wood, 128 },
        { ItemType.Plank, 128 },
        { ItemType.Stone, 128 },
        { ItemType.Block, 128 },
        { ItemType.Diamond, 128 },
        { ItemType.IronOre, 128 },
        { ItemType.Iron, 128 },
        { ItemType.Stairs, 128 },
    };

    private string _id => "test_dict";
    private string _id2 => "test_dict2";
    private string _id3 => "test_dict3";
    private string _id4 => "test_dict4";
    private string _id5 => "test_dict5";
    private string _id6 => "test_dict6";

    private ES3Settings _settings1 = new ES3Settings(ES3.Location.Cache);


    [Button]
    private void CasualSave()
    {
        Save(dictionary => ES3.Save(_id, _testDictionary, _id, _settings1));
    }

    [Button]
    private void PairSave()
    {
        Save(dictionary =>
        {
            foreach (var pair in dictionary)
            {
                ES3.Save(_id2 + $"_{(int)pair.Key}", pair, _id2, _settings1);
            }
        }, "pair");
    }

    [Button]
    private void OnStringSave()
    {
        Save(dictionary =>
        {
            string finalString = "";
            foreach (var pair in _testDictionary)
            {
                finalString += $"{(int)pair.Key}_{pair.Value}|";
            }

            ES3.Save(_id5, finalString, _id5, _settings1);
        }, "one string");
    }
    [Button]
    private void OnIntSave()
    {
        Save(dictionary =>
        {
            ES3.Save(_id6, 2, _id6);
        }, "one int");
    }
    
    [Button]
    private void OnIntSaveCached()
    {
        Save(dictionary =>
        {
            ES3.Save(_id6, 2, _id6, _settings1);
        }, "one int");
    }

    private void Save(Action<Dictionary<ItemType, int>> save, string saveId = "simple")
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        save(_testDictionary);
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Elapsed by {saveId} save {stopwatch.Elapsed.TotalMilliseconds}ms");
    }

    [ES3Serializable]
    public class InternalSave
    {
        [ES3Serializable] public ItemType _itemType;
        [ES3Serializable] public int _count;

        public InternalSave()
        {
            _itemType = ItemType.None;
            _count = 0;
        }
        public InternalSave(ItemType itemType, int count)
        {
            _itemType = itemType;
            _count = count;
        }
    }
}
