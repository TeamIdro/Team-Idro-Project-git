using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myPlayerData = new PlayerData();
    private MapData myMapData = new MapData();


    public void WriteData() 
    {

        myPlayerData.currentHealth = PlayerCharacterController.Instance.hp;
        myPlayerData.playerPos = PlayerCharacterController.Instance.transform.position;
        // SaveGameManager.currentSaveData.playerData = myPlayerData;
        SavePlayerData();
    }

    public void SaveMapData()
    {
        // SaveGameManager.currentSaveData.mapData = myMapData;
        // SaveGameManager.SaveGame();
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
    public float currentHealth;

    //SpellSlot in MagicController.cs

    //Checkpoint?

    //ultima stanza visitata string

    //

}

[System.Serializable]
public struct MapData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public int currentHealth;
}