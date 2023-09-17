using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoKnockableSO", menuName = "Magia/Effetti/EffettoKnockableSO")]
public class EffettoKnockableSO : EffettoBaseSO
{
    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        return;
    }

    public override void ApplicaEffettoANemico(GameObject nemico, Vector2 position)
    {
        return;
    }

    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }

    public override void TogliEffettoANemico(GameObject nemico)
    {
        return;
    }

    public override IEnumerator TogliEffettoDopoDelTempoANemico(GameObject nemico)
    {
        yield return null;
    }
}
