using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{
    public SOScena sceneToReference;
    public bool IsFirstRoom;

    public UnityEvent OnMouseClick;
    public UnityEvent OnRoomEnter;
    

    private void Awake()
    {
        if (sceneToReference == null)
        {
            return;
        }
    }

    public void EnterRoom()
    {
        if (sceneToReference != null)
        {
            SceneManager.LoadScene(sceneToReference.sceneReference);
            Debug.Log(sceneToReference.sceneReference);
            OnRoomEnter.Invoke();
        }
       
    }
    private void OnMouseDown()
    {
        OnMouseClick.Invoke();
    }
    private void OnMouseOver()
    {
        
    }
}
