using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using System;
using Unity.PlasticSCM.Editor.WebApi;

public class UIPauseMenu : MonoBehaviour, ISubscriber
{
    [SerializeField] PauseState currentStateOfPause;
    [Space]
    [SerializeField] GameObject pauseMenuPrefab;


    private GamePlayInputActions inputActions;
    private bool pauseIsActive = false;
    private void Awake()
    {
        inputActions = new GamePlayInputActions();
        if(pauseMenuPrefab!= null)
        {
            if(pauseMenuPrefab.activeInHierarchy) 
            {
                pauseMenuPrefab.SetActive(false);
            }
            currentStateOfPause = PauseState.PauseDeactivated;
        }
    }
    private void OnEnable()
    {
        inputActions.Enable();
        Publisher.Subscribe(this, new StopOnOpenPauseMessage());
        Publisher.Subscribe(this, new ClosePauseMessage());
    }
    private void Update()
    {
        if (inputActions.UI.Attiva_Disattiva_Pausa.WasPressedThisFrame())
        {
            if (currentStateOfPause == PauseState.PauseActivated)
            {
                Debug.Log("Entro nella pressione del tasto pausa 1");

                PubSub.Publisher.Publish(new ClosePauseMessage());
            }
            else if (currentStateOfPause == PauseState.PauseDeactivated)
            {
                Debug.Log("Entro nella pressione del tasto pausa 2");

                PubSub.Publisher.Publish(new StopOnOpenPauseMessage());
            }
        }
    }
    public void OnPublish(IMessage message)
    {
        if(message is StopOnOpenPauseMessage)
        {
            var openPauseMessage = (StopOnOpenPauseMessage)message;
            Debug.Log(openPauseMessage);
            OpenPauseInternal();
            Publisher.Publish(new StopOnOpenPauseMessage());
        }
        else if(message is ClosePauseMessage)
        {
            var closePauseMessage = (ClosePauseMessage)message;
            Debug.Log("UImanager "+closePauseMessage);
            ClosePauseInternal();
            Publisher.Publish(new StartOnClosedPauseMessage());

        }
    }

  
    


    public void OpenPausa()
    {
        PubSub.Publisher.Publish(new StopOnOpenPauseMessage());
    }
    public void ClosePause()
    {
        PubSub.Publisher.Publish(new ClosePauseMessage());
    }
    public void OpenOptions()
    {
        //PubSub.Publisher.Publish()
    }
    private void ClosePauseInternal()
    {
        Time.timeScale = 1;
        currentStateOfPause = PauseState.PauseDeactivated;
        pauseMenuPrefab.SetActive(false);
    }

    private void OpenPauseInternal()
    {
        Time.timeScale = 0;
        currentStateOfPause = PauseState.PauseActivated;
        pauseMenuPrefab.SetActive(true);
    }
    private void OnDisable()
    {
        Publisher.Unsubscribe(this, new StopOnOpenPauseMessage());
        Publisher.Unsubscribe(this, new ClosePauseMessage());

    }
}
public enum PauseState
{
    None,
    PauseActivated,
    PauseDeactivated
}


