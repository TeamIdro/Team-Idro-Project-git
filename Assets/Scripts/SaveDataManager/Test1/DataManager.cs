using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public const string FILENAME = "/GameData.txt";
    public const string DIRECTORY = "/TIPDIR";
    public string FULLPATH;

    private void Start() 
    {
        FULLPATH = Application.persistentDataPath + DIRECTORY + FILENAME;
        if(!Directory.Exists(FULLPATH + DIRECTORY))
        {
            Directory.CreateDirectory(Application.persistentDataPath + DIRECTORY);
        }
    }

    public void SaveData (Data dataToSave)
    {
        string convertedData = JsonUtility.ToJson(dataToSave, true);

        File.WriteAllText(FULLPATH, convertedData);

        print("Succesfully Saved!" + convertedData);
        print("Data Saved in:" + FULLPATH);
    }

    public Data LoadData()
    {
        if (File.Exists(FULLPATH))
        {
            string convertedData = File.ReadAllText(FULLPATH);

            Data reconvertedPlayerData = JsonUtility. FromJson<Data>(convertedData);

            print("Loading Completed");

            return reconvertedPlayerData;
        }

        print("No File To Upload");
        
        return null;
    }

    public void RemoveData()
    {
        if (File.Exists(FULLPATH))
        {
            File.Delete(FULLPATH);

            print("Removed Data");

            return;
        }

        print("No File To Remove");
    }
}
