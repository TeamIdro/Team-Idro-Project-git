using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIMenuPrincipale : MonoBehaviour
{
    private GamePlayInputActions m_gamePlayInputActions;
    private InputAction m_selectUp;
    private InputAction m_selectDown;
    private InputAction m_enterOption;

    public GameObject selectorObj;
    public GameObject selectorUpOrigin;
    public GameObject selectorDownOrigin;
    
    public float maxCastDistance;


    void Awake()
    {
        m_gamePlayInputActions = new GamePlayInputActions();
        
        m_selectUp = m_gamePlayInputActions.UI.SelectUp;
        m_selectDown = m_gamePlayInputActions.UI.SelectDown;
        m_enterOption = m_gamePlayInputActions.UI.EnterOption;

        m_selectUp.Enable();
        m_selectDown.Enable();
        m_enterOption.Enable();

        m_selectUp.performed += Up;
        m_selectDown.performed += Down;
        m_enterOption.performed += EnterOption;
    }

    public void GoToCurrentScene()
    {
        /*Prendi il json e guarda la stringa dell'ultima scena salvata 
         */
    }
    public void NewGamePressed()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void Up(CallbackContext ctx)
    {
        // Debug.Log("up");
        MoveSelector(selectorUpOrigin.transform.position, Vector2.up);
    }

    private void Down(CallbackContext ctx)
    {
        // Debug.Log("down");
        MoveSelector(selectorDownOrigin.transform.position, Vector2.down);
    }

    private void EnterOption(CallbackContext ctx)
    {
        // Debug.Log("enter");
        selectorObj?.transform.parent.gameObject.GetComponent<Button>().onClick.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // Debug.Log("uscito debug");
#else
     Application.Quit(); 
    //  Debug.Log("uscito");
#endif   
    }

    private void MoveSelector(Vector3 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxCastDistance);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            selectorObj.transform.SetParent(hit.collider.transform);
            selectorObj.transform.position = hit.collider.transform.position;
        }
    }

    public void ExitPause()
    {
        foreach (var item in FindObjectsOfType<EnemyScript>())
        {
            item.PauseBehavior();
        }

        foreach (EnemyRangedBullet item in FindObjectsOfType<EnemyRangedBullet>())
        {
            item.StartObject();
        }

        this.gameObject.SetActive(false);

        PlayerCharacterController.Instance.EnableController();

    }
}
