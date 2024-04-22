using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveDataManager
{
    [Serializable]
    class PlayerProfilesData
    {
        public List<string> playerNames;

        public PlayerProfilesData(List<string> names)
        {
            playerNames = names;
        }
    }


    private static string GetSaveDir()
    {
        // C:\Users\Han\AppData\LocalLow\DefaultCompany\InfiniteRunner
        return Application.persistentDataPath;
    }


    private static string GetPlayerProfileFileName()
    {
        return "players.json";
    }


    public static string GetPlayerProfileSaveDir()
    {
        return Path.Combine(GetSaveDir(), GetPlayerProfileFileName());
    }


    public static void SavePlayerProfile(string playerName)
    {
        PlayerProfilesData data = new PlayerProfilesData(new List<string> {playerName});
        string dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPlayerProfileSaveDir(), dataJSON);
    }
}
