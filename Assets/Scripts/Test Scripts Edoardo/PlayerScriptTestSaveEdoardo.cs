using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptTestEdoardo : MonoBehaviour
{
    public Data playerData;
    public DataManager dataManager;

    private void Start() 
    {
        WriteName();     
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SetLoadedData();
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            dataManager.SaveData(playerData);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            dataManager.RemoveData();
        }
    }

    private void WriteName()
    {
        Debug.Log($"Hi, my name is {playerData.objName}");
    }

    void SetLoadedData()
    {
        playerData = dataManager.LoadData();
        Debug.Log($"Object Name is {playerData.objName}");
    }
}
