using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class MovingPlatformButton : MonoBehaviour
{
    [SerializeField] private GameObject movingPlatform;
    [SerializeField] private TipoMagia elementToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("MovingPlatformButton HIT");
        if(other.gameObject.GetComponent<Magia>() != null)
        {
            
            if(elementToActivate == 
                other.gameObject.GetComponent<Magia>().magia.tipoMagia)
            {
                Debug.Log("Activating");
                movingPlatform.GetComponent<MovingPlatform>().moving = true;
            }
        }
    }
}
