using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSpine : MonoBehaviour
{
    [SerializeField, Range(0, 100)] public float dannoSpine;
    [SerializeField, Range(0.2f, 50)] public float waitTick;
    [SerializeField, Range(0f, 5f)] public float velocityMultiplier;

    private float temp = 0f;
    private bool dentroSpine = false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.GetComponent<IDamageable>() != null)
            {
                if(collision.gameObject.GetComponent<PlayerCharacterController>() != null && dentroSpine == false)
                {
                    dentroSpine = true;
                    PlayerCharacterController controller = collision.gameObject.GetComponent<PlayerCharacterController>();
                    StartCoroutine(DannoATickSpine(controller));
                    temp = controller.MageVelocity;
                    controller.MageVelocity = controller.MageVelocity / velocityMultiplier;
                    Debug.Log("ENTRO NELLE SPINE, ENTRATA: " + temp);
                }
                else if(collision.gameObject.GetComponent<EnemyScript>() != null) 
                {
                    return;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerCharacterController>() != null && dentroSpine == true)
            {
                dentroSpine = false;
                PlayerCharacterController controller = collision.gameObject.GetComponent<PlayerCharacterController>();
                controller.MageVelocity = temp * velocityMultiplier;
                Debug.Log("ENTRO NELLE SPINE, VELOCITA USCITA: " + temp);
            }
        }
       
        StopAllCoroutines();
    }

    private IEnumerator DannoATickSpine(IDamageable danneggiabile)
    {
        while(true)
        {
            danneggiabile.TakeDamage(dannoSpine, TipoMagia.Veleno);
            yield return new WaitForSeconds(waitTick);
        }
    }
}
