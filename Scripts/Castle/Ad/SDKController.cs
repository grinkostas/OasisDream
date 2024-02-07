using HomaGames.HomaBelly;

public static class SDKController
{
    public static void SendEvent(string eventValue)
    {
        Analytics.DesignEvent(eventValue);
    }

    public static void LevelStart(string eventValue)
    {
        Analytics.DesignEvent($"{eventValue}_started");
    }

    public static void LevelComplete(string eventValue)
    {
        Analytics.DesignEvent($"{eventValue}_completed");
    }
    
}
