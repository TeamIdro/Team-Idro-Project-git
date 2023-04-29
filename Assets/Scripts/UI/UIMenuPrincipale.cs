using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIMenuPrincipale : MonoBehaviour
{
    public UnityEvent onNewGamePressed;
    public void GoToCurrentScene()
    {
        /*Prendi il json e guarda la stringa dell'ultima scena salvata 
         */
    }
    public void NewGamePressed()
    {
        onNewGamePressed.Invoke();
        SceneManager.LoadSceneAsync(1);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("uscito debug");
#else
     Application.Quit(); 
     Debug.Log("uscito")
#endif   
    }
}
