using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Loading;

public class TutorialLoadingOperation : LoadingOperation
{
    [SerializeField] private string _tutorialSaveId;

    public override void Load()
    {
        if (ES3.Load(_tutorialSaveId, false) == false)
            ES3.DeleteFile();
        Finish();
    }
}