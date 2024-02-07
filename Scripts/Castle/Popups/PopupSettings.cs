using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class PopupSettings : Settings<PopupSettings>
{
    public List<PopupData> PopupPrefabs;

    public PopupAnimationData In;
    public PopupAnimationData Out;

    public PopupData GetData<TPopup>() where TPopup : APopup
    {
        return PopupPrefabs.Find(x => x.Popup.Type == typeof(TPopup));
    }

    public PopupData GetData(string id)
    {
        return PopupPrefabs.Find(x => x.Id == id);
    }

}

[Serializable]
public class PopupData
{
    public string Id;
    public APopup Popup;

    public PopupData()
    {
        Id = "Empty";
        Popup = null;
    }

    public PopupData(string id, APopup popup)
    {
        Id = id;
        Popup = popup;
    }
}

[Serializable]
public class PopupAnimationData
{
    public float Duration;
    public Ease FadeEase;
    public Ease ZoomEase;
}
