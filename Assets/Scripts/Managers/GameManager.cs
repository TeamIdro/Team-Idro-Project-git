using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class GameManager : Manager
{
    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
                if (m_instance == null)
                {
                    GameObject nuovoGameManager = new GameObject("GameManager", typeof(GameManager));
                    m_instance = nuovoGameManager.GetComponent<GameManager>();
                }
            }
            return m_instance;
        }
    }
    private void InitiliazeManager()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void InitiliazeVariables()
    {
        throw new NotImplementedException();
    }
    [Header("Player Reference(Chess Version)")]
    public GameObject playerPrefab_ChessVersion;
    [Header("Player Reference(Layton Type Version)")]
    public GameObject playerPrefab_LaytonVersion;
    [Space(20)]
    [Header("Virtual Camera For Kid Chess Version")]
    public GameObject virtualCameraForChessKid;


    [ReadOnly] public GameStates currentGameState = GameStates.PlayerOnChessBoardScene;



    public delegate void OnChangedScene();
    public static event OnChangedScene onChangedSceneEvent;
    



    private void Awake()
    {
        InitiliazeManager();
        InitiliazeVariables();
    }

   

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        currentGameState = GameStates.Menù;
        SetCameras();
    }
    private void SetCameras()
    {
        //set camera based on the current state of the game
    }

}
