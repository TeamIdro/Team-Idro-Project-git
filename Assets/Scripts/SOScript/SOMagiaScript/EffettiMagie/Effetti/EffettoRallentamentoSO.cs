using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoRallentamentoSO", menuName = "Magia/Effetti/Effetto Rallentamento SO")]
public class EffettoRallentamentoSO : EffettoBaseSO
{
    [Range(0,100)]
    public int percentualeRallentamentoNemicoColpito = 0;
    [Range(0, 100)]
    public float moltiplicatoreDiMagia;
    private float valoreOriginaleNemici;


    private void Awake()
    {
        
    }
    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        return;
    }
    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }

    public override void ApplicaEffettoANemico(GameObject target,Vector2 position)
    {
        Debug.LogWarning("EFFETTO APPLICATO");
        if (target.GetComponent<EnemyScript>())
        {
            EnemyScript enemyScript = target.GetComponent<EnemyScript>();
            int valoreRandomicoPerPercentuale = Random.Range(0, 101);
            if(valoreRandomicoPerPercentuale <= percentualeRallentamentoNemicoColpito)
            {
                valoreOriginaleNemici = enemyScript.speed;
                enemyScript.speed /= moltiplicatoreDiMagia;
            }

        }
        else if (target.GetComponent<MovingPlatform>() && target.GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
            MovingPlatform temp = agent.GetComponent<MovingPlatform>();
            if(temp.isAffectedByEffect == false)
            {
                int valoreRandomicoPerPercentuale = Random.Range(0, 101);
                if (valoreRandomicoPerPercentuale <= percentualeRallentamentoNemicoColpito)
                {
                    float percentuale = moltiplicatoreDiMagia / 100;
                    valoreOriginaleNemici = agent.speed;
                    agent.speed *= percentuale;
                    temp.isAffectedByEffect = true;
                }

            }
            temp.StartCoroutine(TogliEffettoDopoDelTempoANemico(target));
        }
       
    }

    public override IEnumerator TogliEffettoDopoDelTempoANemico(GameObject target)
    {
        float durataEffettoLocale = durataEffetto;
        yield return new WaitForSecondsRealtime(durataEffettoLocale);
        if(target.GetComponent<EnemyScript>() != null)
        {
            EnemyScript enemyScript = target.GetComponent<EnemyScript>();
            target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            enemyScript.speed = valoreOriginaleNemici;
        }
        else if(target.GetComponent<MovingPlatform>() != null)
        {
            MovingPlatform temp = target.GetComponent<MovingPlatform>();
            NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
            temp.isAffectedByEffect = false;
            agent.speed = valoreOriginaleNemici;
        }
        yield return null;
    }

    public override void TogliEffettoANemico(GameObject target)
    {
        float durataEffettoLocale = durataEffetto;
        if (target.GetComponent<EnemyScript>() != null)
        {
            EnemyScript enemyScript = target.GetComponent<EnemyScript>();
            target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            enemyScript.speed = valoreOriginaleNemici;
        }
        else if (target.GetComponent<MovingPlatform>() != null)
        {
            MovingPlatform temp = target.GetComponent<MovingPlatform>();
            NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
            temp.isAffectedByEffect = false;
            agent.speed = valoreOriginaleNemici;
        }
    }
}
