using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveGameManager
{
    public static SaveData currentSaveData = new SaveData();

    public const string FILENAME = "/GameData.txt";
    public const string DIRECTORY = "/TIPDIR";
    
    public static bool SaveGame()
    {
        string  dir = Application.persistentDataPath + DIRECTORY;
        Debug.Log("DIR: " + dir);

        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(currentSaveData, true);

        File.WriteAllText(dir + FILENAME, json);

        return true;
    }

    public static void LoadGame()
    {
        string fullPath = Application.persistentDataPath + DIRECTORY + FILENAME;

        SaveData tempData = new SaveData();

        if(File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            tempData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogError("Save File Doesn't Exist!");
        }

        currentSaveData = tempData;
    }
}
