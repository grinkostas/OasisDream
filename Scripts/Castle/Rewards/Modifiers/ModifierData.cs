using System;

[Serializable] 
public class ModifierData
{
    public string ModifierId;
    public DateTime StartTime;
    public float Duration;

    public ModifierData()
    {
        
    }
    
    public ModifierData(string modifierId, DateTime startTime, float duration)
    {
        ModifierId = modifierId;
        StartTime = startTime;
        Duration = duration;
    }

    public void Save()
    {
        
    }
}
