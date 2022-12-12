using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myPlayerData = new PlayerData();
    private MapData myMapData = new MapData();

    // void Update()
    // {
    //     // myData.playerPos = transform.position;
    //     // myData.playerRot = transform.rotation;
    //     // myData.currentHealth = 10;

    //     if(Input.GetKeyDown(KeyCode.Q))
    //     {
    //         // SaveGameManager.currentSaveData.playerData = myData;
    //         SaveGameManager.SaveGame();
    //     }

    //     if(Input.GetKeyDown(KeyCode.L))
    //     {
    //         SaveGameManager.LoadGame();
    //         // myData = SaveGameManager.currentSaveData.playerData;
    //         // transform.position = myData.playerPos;
    //         // transform.rotation = myData.playerRot;
    //     }
    // }

    public void SaveMapData()
    {
        SaveGameManager.currentSaveData.mapData = myMapData;
        SaveGameManager.SaveGame();
    }

    public void SavePlayerData()
    {
        SaveGameManager.currentSaveData.playerData = myPlayerData;
        SaveGameManager.SaveGame();
    }

    public void LoadMapData()
    {
        SaveGameManager.LoadGame();
    }

    public void LoadPlayerData()
    {
        SaveGameManager.LoadGame();
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public int currentHealth;
}

[System.Serializable]
public struct MapData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public int currentHealth;
}