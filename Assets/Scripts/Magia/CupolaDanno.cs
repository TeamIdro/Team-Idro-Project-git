using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CupolaDanno : MonoBehaviour
{
    [HideInInspector] public float tickCupola;
    [HideInInspector] public float raggioCupola;
    [HideInInspector] public int dannoCupola;
    [HideInInspector] public float durataCupola;
    [SerializeField] List<EffettoBaseSO> effettiCupola;

    EnemyScript nemicoACuiApplicareIlDanno;

    private void Start()
    {
       
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if(collision.gameObject.GetComponent<EnemyScript>() != null)
            {
                nemicoACuiApplicareIlDanno= collision.gameObject.GetComponent<EnemyScript>();
                foreach (var effetto in effettiCupola)
                {
                    effetto.ApplicaEffettoANemico(nemicoACuiApplicareIlDanno);
                    effetto.TogliEffettoDopoDelTempoANemico(nemicoACuiApplicareIlDanno);
                }
                Debug.LogWarning("DANNO AL NEMICO DA CUPOLA");
                nemicoACuiApplicareIlDanno.TakeDamage(dannoCupola, TipoMagia.Fuoco);
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
}
