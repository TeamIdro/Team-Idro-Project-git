using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myData = new PlayerData();

    // Update is called once per frame
    void Update()
    {
        myData.playerPos = transform.position;
        myData.playerRot = transform.rotation;
        myData.currentHealth = 10;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            SaveGameManager.currentSaveData.playerData = myData;
            SaveGameManager.SaveGame();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            SaveGameManager.LoadGame();
            myData = SaveGameManager.currentSaveData.playerData;
            transform.position = myData.playerPos;
            transform.rotation = myData.playerRot;

        }
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public int currentHealth;

}
