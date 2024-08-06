using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    private void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(2735690);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
        foreach (var a in Steamworks.SteamUserStats.Achievements)
        {
            Debug.Log($"{a.Name} ({a.State})");
        }
    }
    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }


    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    public static bool IsUnlockedAchievement(string id)
    {
        if (!Steamworks.SteamClient.IsValid)
        {
            return false;
        }
        var ach = new Steamworks.Data.Achievement(id);
        return ach.State;
    }
    public void IsThisAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);

        Debug.Log($"Achievement {id} BatteryStatus: " + ach.State);
    }

    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();

        Debug.Log($"Achievement {id} cleared");
    }
    public static void UnlockAchievement(string id)
    {
        if (!Steamworks.SteamClient.IsValid)
        {
            return;
        }
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        StoreStat();

        Debug.Log($"Achievement {id} unlocked");
    }

    static public void AddStat(string APIName, int amount)
    {
        if (!Steamworks.SteamClient.IsValid)
        {
            return;
        }
        Steamworks.SteamUserStats.AddStat(APIName, amount);
        Debug.Log(APIName + " " + Steamworks.SteamUserStats.GetStatInt(APIName));
        StoreStat();
    }
    
    static public void StoreStat()
    {
        Steamworks.SteamUserStats.StoreStats();
    }
}
