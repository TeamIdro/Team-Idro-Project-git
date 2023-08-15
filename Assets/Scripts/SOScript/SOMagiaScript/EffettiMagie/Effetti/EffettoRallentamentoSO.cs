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

    public override void ApplicaEffettoANemico(EnemyScript danneggiabile)
    {
        Debug.LogWarning("EFFETTO APPLICATO");
        int valoreRandomicoPerPercentuale = Random.Range(0, 101);
        if(valoreRandomicoPerPercentuale <= percentualeRallentamentoNemicoColpito)
        {
            valoreOriginaleNemici = danneggiabile.speed;
            danneggiabile.speed /= moltiplicatoreDiMagia;
            Renderer renderer = danneggiabile.gameObject.GetComponent<Renderer>();
            Material material = renderer.material;
            if (material != null)
            {
                material.SetFloat("_OutlineThickness", 1f);
                material.SetColor("_OutlineColor", coloreEffetto);
                renderer.material = material;
            }
        }
    }


    public override IEnumerator TogliEffettoDopoDelTempoANemico(EnemyScript nemico)
    {
        yield return new WaitForSeconds(durataEffetto);
        nemico.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Debug.LogWarning("EFFETTO TOLTO");
        nemico.speed = valoreOriginaleNemici;
        Renderer renderer = nemico.gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        if(material!= null)
        {
            material.SetFloat("_OutlineThickness", 0);
            material.SetColor("_OutlineColor", Color.white);
            renderer.material = material;

        }
    }
}
