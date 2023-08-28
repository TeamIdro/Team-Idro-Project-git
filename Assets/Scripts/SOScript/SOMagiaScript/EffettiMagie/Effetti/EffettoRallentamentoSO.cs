using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoRallentamentoSO", menuName = "Magia/Effetti/Effetto Rallentamento SO")]
public class EffettoRallentamentoSO : EffettoBaseSO
{
    [Range(0,100)]
    public int percentualeRallentamentoNemicoColpito = 0;
    [Range(0, 10)]
    public int moltiplicatoreDiMagia;

    private float valoreOriginaleNemici;

    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        return;
    }
    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }

    public override void ApplicaEffettoANemico(GameObject target)
    {
        Debug.LogWarning("EFFETTO APPLICATO");
        EnemyScript enemyScript = target.GetComponent<EnemyScript>();
        int valoreRandomicoPerPercentuale = Random.Range(0, 101);
        if(valoreRandomicoPerPercentuale <= percentualeRallentamentoNemicoColpito)
        {
            valoreOriginaleNemici = enemyScript.speed;
            enemyScript.speed /= moltiplicatoreDiMagia;
            Renderer renderer = target.gameObject.GetComponent<Renderer>();
            Material material = renderer.material;
            if (material != null)
            {
                material.SetFloat("_OutlineThickness", 1f);
                material.SetColor("_OutlineColor", coloreEffetto);
                renderer.material = material;
            }
        }
    }


    public override IEnumerator TogliEffettoDopoDelTempoANemico(GameObject target)
    {
        yield return new WaitForSeconds(durataEffetto);
        EnemyScript enemyScript = target.GetComponent<EnemyScript>();
        target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Debug.LogWarning("EFFETTO TOLTO");
        enemyScript.speed = valoreOriginaleNemici;
        Renderer renderer = target.gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        if(material!= null)
        {
            material.SetFloat("_OutlineThickness", 0);
            material.SetColor("_OutlineColor", Color.white);
            renderer.material = material;

        }
    }
}
