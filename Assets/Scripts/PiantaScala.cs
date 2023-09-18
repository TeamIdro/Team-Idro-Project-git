using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiantaScala : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            Debug.Log("è entrato il player sulle scale");
            PlayerCharacterController.Instance.IsOnStairs = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision);
        if(collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            Debug.Log("è entrato il player sulle scale");
            PlayerCharacterController.Instance.IsOnStairs = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            Debug.Log("è uscito il player sulle scale");
            PlayerCharacterController.Instance.IsOnStairs = false;
        }
    }
}
