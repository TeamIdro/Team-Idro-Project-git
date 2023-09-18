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
        Debug.Log("TATSO PAUSA");
        if(PauseUI 
            && !PauseUI.activeInHierarchy)
        {
            PlayerCharacterController.Instance.DisableControl();

            PauseUI.SetActive(true);

            // Pause the behavior of all enemy scripts
            foreach (EnemyScript item in FindObjectsOfType<EnemyScript>())
            {
                item.PauseBehavior();
            }

            foreach (EnemyRangedBullet item in FindObjectsOfType<EnemyRangedBullet>())
            {
                item.PauseObject();
            }

        }
    }

    private void OnDisable()
    {
        m_activePause.performed -= Pause;
        m_activePause.Disable();

    }
}
