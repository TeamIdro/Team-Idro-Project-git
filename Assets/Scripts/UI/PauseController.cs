using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class PauseController : MonoBehaviour
{
    private GamePlayInputActions m_gamePlayInputActions;
    private InputAction m_activePause;
    public GameObject PauseUI;

    void Awake()
    {
        m_gamePlayInputActions = new GamePlayInputActions();
        m_activePause = m_gamePlayInputActions.UI.Attiva_Disattiva_Pausa;
        m_activePause.Enable();
        m_activePause.performed += Pause;
        
    }

    private void Pause(CallbackContext ctx)
    {
        if(!PauseUI.activeInHierarchy)
        {
            PauseUI.SetActive(true);

            foreach (var item in FindObjectsOfType<EnemyScript>())
            {
                item.PauseBehavior();
            }
        }
    }
}
