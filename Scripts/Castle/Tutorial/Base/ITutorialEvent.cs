using UnityEngine.Events;

public interface ITutorialEvent
{
    UnityAction Finished { get; set; }
    UnityAction Available { get; set; }
    UnityAction<float> ProgressChanged { get; set; }
    
    float Progress { get; }
    float FinalValue { get; }

    bool IsFinished();
    bool IsAvailable();
}
