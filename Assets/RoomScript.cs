using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{
    public SceneAsset roomData;
    public bool IsFirstRoom;

    private static string roomName;
    public static UnityEvent OnRoomEnter;
   

    private void Awake()
    {
        roomName = roomData.name;
    }

    public static void EnterRoom()
    {
        SceneManager.LoadScene(roomName);
        OnRoomEnter.Invoke();
    }
}
