using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

internal enum TurretDirection
{
    Sinistra,
    Destra,
}
public class ElementTurret : MonoBehaviour
{
    [Header("Valori per settare il behaviour dello sparo")]
    [SerializeField] TurretDirection direzioneTorretta;
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
    [Range(0f, 30f)] public float danno;
    [SerializeField] float tempoTickPerDanno;

    [Tooltip("Prefab del proiettile base che poi la torretta sparerà")]
    [SerializeField] GameObject elementoDaSpawnare;
    [Tooltip("Tipo del'elemento che la magia spara")]
    [SerializeField] ElementoMagiaSO elementoDaUsareSO;
    [SerializeField] LayerMask layerDaColpire;
    [SerializeField] ObjectForTurret objectForTurret = new ObjectForTurret();


    float durataAttaccoLocale = 0;
    public delegate void CoroutineCallback();
    public event CoroutineCallback callback = delegate { };
    private void Awake()
    {
        InvokeRepeating("DannoATick", 0.1f, tempoTickPerDanno);
    }
    private void Start()
    {
        durataAttaccoLocale = durataAttacco;
        StartCoroutine(IniziaAttacco(() =>
        {
            elementoDaSpawnare.GetComponent<ParticleSystem>().Stop();
        }));
        elementoDaSpawnare.transform.parent = puntoDiSparo;
        if (direzioneTorretta == TurretDirection.Sinistra)
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        else
            gameObject.transform.rotation = new Quaternion(0, -180, 0, 0);
    }
    private void OnValidate()
    {
        if(circleColliderPerDetectIDamageable != null)
        {
            circleColliderPerDetectIDamageable.radius = raggioCircleCollider;
            circleColliderPerDetectIDamageable.offset = offsetCollider;
        }
        if(direzioneTorretta == TurretDirection.Sinistra)
            gameObject.transform.rotation = new Quaternion(0,0,0,0);
        else
            gameObject.transform.rotation = new Quaternion(0,-180,0,0);
    }
    // PER CAPIRE SE IL PLAYER O UN NEMICO è DENTRO IL TRIGGER INIZIARE L'ATTACCO
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            objectForTurret.collision = collision;
            objectForTurret.isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectForTurret.collision = null;
        objectForTurret.isInRange = false;
    }

    private IEnumerator IniziaAttacco( CoroutineCallback callback)
    {
        while (durataAttaccoLocale > 0.0f)
        {
            durataAttaccoLocale -= Time.deltaTime;
            Debug.Log(objectForTurret.collision != null
                && objectForTurret.collision.gameObject.GetComponent<IDamageable>() != null
                && objectForTurret.isInRange == true);
            if (objectForTurret.collision != null 
                && objectForTurret.collision.gameObject.GetComponent<IDamageable>() != null
                && objectForTurret.isInRange == true)
            {
                
            }
            if (durataAttaccoLocale <= 0.0f)
            {
                durataAttaccoLocale = durataAttacco;
                callback.Invoke();
                yield return new WaitForSeconds(durataPausa);
                elementoDaSpawnare.GetComponent<ParticleSystem>().Play();

            }

            yield return null;
        }
    }
    private void DannoATick()
    {
        if (objectForTurret.isInRange && objectForTurret.collision != null)
        {
            IDamageable damageableObject = objectForTurret.collision.gameObject.GetComponent<IDamageable>();

            if (damageableObject != null && elementoDaSpawnare.GetComponent<ParticleSystem>().isPlaying)
            {
                Debug.Log("FAI DANNO ");
                damageableObject.TakeDamage(danno, elementoDaUsareSO.tipoDiMagia);
            }
        }
    }






}



[Serializable]
public struct ObjectForTurret
{
    public bool isInRange;
    public Collider2D collision;
}
