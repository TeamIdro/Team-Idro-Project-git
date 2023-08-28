using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CupolaDanno : MonoBehaviour
{
    [SerializeField] private float tickCupola;
    [SerializeField] private float raggioCupola;
    [SerializeField]private int dannoCupola;
    [SerializeField] private float durataCupola;
    [SerializeField] List<EffettoBaseSO> effettiCupola;

    EnemyScript nemicoACuiApplicareIlDanno;

    private void Awake()
    {
        ParticleSystem particleSystemCupola = gameObject.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystemCupola.main;
        mainModule.startLifetime = durataCupola;
        mainModule.startSize = raggioCupola* 2;
        SetRaggioCupola(raggioCupola);
        Destroy(gameObject, durataCupola);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if(collision.gameObject.GetComponent<EnemyScript>() != null)
            {
                nemicoACuiApplicareIlDanno = collision.gameObject.GetComponent<EnemyScript>();
                foreach (var effetto in effettiCupola)
                {
                    effetto.ApplicaEffettoANemico(nemicoACuiApplicareIlDanno.gameObject);
                    effetto.TogliEffettoDopoDelTempoANemico(nemicoACuiApplicareIlDanno.gameObject);
                }
                StartCoroutine(DannoATickCupola());
               
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        nemicoACuiApplicareIlDanno = null;
    }

    public void Update()
    {
        
    }
    public void SetRaggioCupola(float raggioCupola)
    {
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = raggioCupola;
    }
    public void SetDannoCupola(int dannoCupola)
    {
        this.dannoCupola = dannoCupola;
    }
    public IEnumerator DannoATickCupola()
    {
        while(nemicoACuiApplicareIlDanno != null)
        {
            nemicoACuiApplicareIlDanno.TakeDamage(dannoCupola, TipoMagia.Fuoco);
            yield return new WaitForSeconds(tickCupola);
        }
    }
}
