using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlatform : MonoBehaviour
{
    [Header("Moltiplicatori di Salto")]
    [Range(1,3)]
    public float moltiplicatoreMassimoDiSalto = 1;


    public bool isOnPlatform = false;
    float startTime;
    float pressDuration;
    private GamePlayInputActions inputActions;
    private PlayerCharacterController playerCaching;
    private void Awake()
    {
        inputActions = new GamePlayInputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable(); 
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    private void Start()
    {
        inputActions.Mage.Jump.started += ctx => PrendoIlTempoDiPartenza(ctx);
        inputActions.Mage.Jump.canceled+= ctx => FaiIlCalcoloDelJump(ctx);
        
    }
    private void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            Debug.Log("OnTriggerEnter2D JumpPlatform");
            playerCaching = collision.gameObject.GetComponent<PlayerCharacterController>();
            isOnPlatform= true;
            playerCaching.ToggleJump(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            Debug.Log("OnTriggerExit2D JumpPlatform");
            isOnPlatform = false;
            playerCaching.ToggleJump(true);
            playerCaching = null;

        }
    }


    private void PrendoIlTempoDiPartenza(InputAction.CallbackContext obj)
    {
        Debug.Log("PrendoIlTempoDiPartenza JumpPlatform");
        if (playerCaching != null)
        {
            startTime = Time.time;
        }
    }
    private void FaiIlCalcoloDelJump(InputAction.CallbackContext obj)
    {
        Debug.Log("FaiIlCalcoloDelJump JumpPlatform");
        if (playerCaching != null)
        {
            pressDuration = Time.time - startTime;
            Debug.Log(pressDuration);
            if(pressDuration < 1)
            {
                pressDuration = 1;
            }
            else if (pressDuration > 3) 
            {
                pressDuration = 3;
            }
            playerCaching.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, playerCaching.jumpVelocity * moltiplicatoreMassimoDiSalto * pressDuration));

        }
    }
}
