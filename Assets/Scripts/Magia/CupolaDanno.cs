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

    [Space(20)]
    [SerializeField] private ParticleSystem sparkFade;
    [SerializeField] private ParticleSystem snowFlakes;
    [SerializeField] private ParticleSystem iceClouds;
    
    EnemyScript nemicoACuiApplicareIlDanno;
    Platform piattaformaDaSpostare;
    private void Awake()
    {
        ParticleSystem particleSystemCupola = gameObject.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainModuleCupolaPrincipale = particleSystemCupola.main;

        ParticleSystem.MainModule mainModuleSnow = snowFlakes.main;
        ParticleSystem.EmissionModule emissionModuleSnow = snowFlakes.emission;
        emissionModuleSnow.rateOverTime = (raggioCupola * 2) * 2;

        mainModuleSnow.startLifetime = durataCupola;
        mainModuleCupolaPrincipale.startLifetime = durataCupola;
        mainModuleCupolaPrincipale.startSize = raggioCupola * 2;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if(collision.gameObject.GetComponent<EnemyScript>() != null || collision.gameObject.GetComponent<Platform>() != null)
            {
                Debug.Log("ENTRATO "+ collision.gameObject.name);
                if(collision.gameObject.GetComponent<EnemyScript>() != null)
                {
                    nemicoACuiApplicareIlDanno = collision.gameObject?.GetComponent<EnemyScript>();
                    foreach (var effetto in effettiCupola)
                    {
                        effetto.ApplicaEffettoANemico(nemicoACuiApplicareIlDanno.gameObject, gameObject.transform.position);
                        StartCoroutine(effetto.TogliEffettoDopoDelTempoANemico(nemicoACuiApplicareIlDanno.gameObject));
                    }
                }
                else if(collision.gameObject.GetComponent<Platform>() != null)
                {
                    piattaformaDaSpostare = collision.gameObject?.GetComponent<Platform>();
                    foreach (var effetto in effettiCupola)
                    {
                        if(effetto is EffettoRallentamentoSO)
                        {
                            effetto.ApplicaEffettoANemico(piattaformaDaSpostare.gameObject, gameObject.transform.position);
                        }
                        else
                        {
                            effetto.ApplicaEffettoANemico(piattaformaDaSpostare.gameObject, gameObject.transform.position);
                            StartCoroutine(effetto.TogliEffettoDopoDelTempoANemico(piattaformaDaSpostare.gameObject));
                        }
                    }
                }
               
                StartCoroutine(DannoATickCupola());
               
            }
        }
    }
    private void OnDestroy()
    {
        foreach (var effetto in effettiCupola)
        {
            Debug.LogWarning("CIAOO");
            if(piattaformaDaSpostare != null)
            {
                effetto.TogliEffettoANemico(piattaformaDaSpostare.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        nemicoACuiApplicareIlDanno = null;
        piattaformaDaSpostare = null;
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