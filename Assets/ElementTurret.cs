using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ElementTurret : MonoBehaviour
{
    [Header("Valori per settare il behaviour dello sparo")]
    [SerializeField] Transform puntoDiSparo;
    [Tooltip("Lunghezza del \"proiettile\"")]
    [SerializeField] float distanzaProiettile;
    [Tooltip("Tempo in secondi prima che l'attacco finisca")]
    [SerializeField] float durataAttacco;
    [Tooltip("Tempo in secondi prima che la torretta attacchi di nuovo")]
    [SerializeField] float durataPausa;
    [SerializeField] CircleCollider2D circleColliderPerDetectIDamageable;
    [SerializeField, Range(1, 30)] public float raggioCircleCollider;
    [SerializeField] Vector2 offsetCollider;

    [Header("Valori per settare danno e prefab dello sparo")]
    [SerializeField]
    [Range(0f, 30f)] public int danno;

    [Tooltip("Prefab del proiettile base che poi la torretta sparerà")]
    [SerializeField] GameObject elementoDaSpawnare;
    [Tooltip("Tipo del'elemento che la magia spara")]
    [SerializeField] ElementoMagiaSO elementoDaUsareSO;
    [SerializeField] LayerMask layerDaColpire;
    private ObjectForTurret objectForTurret = new ObjectForTurret();



    public delegate void CoroutineCallback();
    public event CoroutineCallback callback;
    private void Start()
    {
        StartCoroutine(IniziaAttacco(objectForTurret, callback));
        elementoDaSpawnare.transform.parent = puntoDiSparo;
    }
    private void OnValidate()
    {
        if(circleColliderPerDetectIDamageable != null)
        {
            circleColliderPerDetectIDamageable.radius = raggioCircleCollider;
            circleColliderPerDetectIDamageable.offset = offsetCollider;
        }
    }
    // PER CAPIRE SE IL PLAYER O UN NEMICO è DENTRO IL TRIGGER INIZIARE L'ATTACCO
    private void OnTriggerEnter2D(Collider2D collision)
    {
        callback += ElementTurret_callback;
        objectForTurret.collision = collision;
        objectForTurret.isInRange = true;
    }

    private void ElementTurret_callback()
    {
        elementoDaSpawnare.GetComponent<ParticleSystem>().Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectForTurret.collision = collision;
        objectForTurret.isInRange = false;
    }


    private IEnumerator IniziaAttacco(ObjectForTurret objectForTurretInizioAttacco, CoroutineCallback callback)
    {
        while (true)
        {
            elementoDaSpawnare.GetComponent<ParticleSystem>().Play();  
            Debug.Log("MESSAGGIO ATTACCO");
            if (objectForTurretInizioAttacco.isInRange == false || objectForTurretInizioAttacco.collision == null)
                yield break;

            if (objectForTurretInizioAttacco.collision.gameObject.GetComponent<PlayerCharacterController>() != null)
            {
                if (!LayerMaskExtensions.IsInLayerMask(objectForTurretInizioAttacco.collision.gameObject, layerDaColpire))
                {
                    yield break;
                }
                elementoDaSpawnare.SetActive(true);
                Debug.Log("spawna il proiettile e mettilo come figlio");
                Collider2D colliderProiettileSpawnato = elementoDaSpawnare.GetComponent<Collider2D>();
                
            }
            yield return new WaitForSeconds(durataPausa);
            callback.Invoke();
            yield return StartCoroutine(IniziaAttacco(objectForTurretInizioAttacco, callback));
        }
        
    }
}
public struct ObjectForTurret
{
    public bool isInRange;
    public Collider2D collision;
}
