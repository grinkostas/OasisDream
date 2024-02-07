//using HomaGames.HomaBelly;
using UnityEngine;
using StaserSDK.Loading;

public class AnalyticsLoadingOperation : LoadingOperation
{
    public override void Load()
    {
        //if (HomaBelly.Instance.IsInitialized)
        {
            Finish();
            return;
        }
        
        //Events.onInitialized += OnInitialized;
    }

    private void OnInitialized()
    {
       // Events.onInitialized -= OnInitialized;
        Finish();
    }
}
