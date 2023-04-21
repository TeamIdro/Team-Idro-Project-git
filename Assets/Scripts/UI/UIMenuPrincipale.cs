using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuPrincipale : MonoBehaviour
{
    
    public void Start()
    {
        
    }

    public void Update()
    {
        
    }
    public void GoToCurrentScene()
    {
        /*Prendi il json e guarda la stringa dell'ultima scena salvata 
         */
    }
    public void NewGamePressed()
    {
         SceneManager.LoadSceneAsync(0);
    }
}
