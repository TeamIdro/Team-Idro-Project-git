using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoRallentamentoSO", menuName = "Magia/Effetti/EffettoRallentamentoSO")]
public class EffettoRallentamentoSO : EffettoBaseSO
{
    [Range(0, 100)]
    public int percentualeRallentamentoNemicoColpito = 0;
    public float durataRallentamento = 0f;
    [Range(0, 10)]
    public int moltiplicatoreDiMagia;

    private float valoreOriginaleNemici;
    public override void ApplicaEffetto(EnemyScript danneggiabile)
    {
        Debug.LogWarning("EFFETTO APPLICATO");
        int valoreRandomicoPerPercentuale = Random.Range(0, 101);
        if(valoreRandomicoPerPercentuale <= percentualeRallentamentoNemicoColpito)
        {
            valoreOriginaleNemici = danneggiabile.speed;
            danneggiabile.speed /= moltiplicatoreDiMagia; 
        }
    }
    public override void TogliEffetto(EnemyScript danneggiabile)
    {
        danneggiabile.speed = valoreOriginaleNemici;
    }

    public override IEnumerator TogliEffettoDopoDelTempo(EnemyScript nemico)
    {
        yield return new WaitForSeconds(durataRallentamento);
        Debug.LogWarning("EFFETTO TOLTO");
        nemico.speed = valoreOriginaleNemici;
    }
}
