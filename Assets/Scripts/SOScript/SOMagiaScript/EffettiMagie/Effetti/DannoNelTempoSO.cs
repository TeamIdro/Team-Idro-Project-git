using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/Effetti/DannoNelTempoSO", menuName = "Magia/Effetti/Danno Nel Tempo")]
public class DannoNelTempoSO : EffettoBaseSO
{
    public int quantitaDiDanniNelTempo;
    [ReadOnly] public TipoMagia tipoMagiaDannoNelTempo;
    public float tempoTick;
    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        return;
    }
    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }
    public override void ApplicaEffettoANemico(EnemyScript nemico)
    {
        Renderer renderer = nemico.gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetFloat("_OutlineThickness", 1f);
        material.SetColor("_OutlineColor", coloreEffetto);
        nemico.StartCoroutine(DanniATick(nemico));
    }
    public IEnumerator DanniATick(EnemyScript nemico)
    {
        int quantitaDITick = Mathf.FloorToInt(durataEffetto / tempoTick);
        for (int i = 0; i < quantitaDITick; i++)
        {
            Debug.LogWarning("DANNO TICK "+ i);
            nemico.TakeDamage(quantitaDiDanniNelTempo, tipoMagiaDannoNelTempo);
            yield return new WaitForSeconds(tempoTick);
        }
    }
    public override IEnumerator TogliEffettoDopoDelTempoANemico(EnemyScript nemico)
    {
        yield return new WaitForSeconds(durataEffetto);
        Renderer renderer = nemico.gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetFloat("_OutlineThickness", 0);
        material.SetColor("_OutlineColor", Color.white);
        nemico.StopCoroutine(DanniATick(nemico));
    }
}
