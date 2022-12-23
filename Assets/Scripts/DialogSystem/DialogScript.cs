using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogScript : MonoBehaviour
{
    PlayerInput playerInput;


    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnNextSentence()
    {
        //NEXT SENTENCE
        DialogController.Instance.NextSentence();

    }
}
