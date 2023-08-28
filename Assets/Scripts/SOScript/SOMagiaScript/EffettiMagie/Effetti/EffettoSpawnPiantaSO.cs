using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoSpawnPiantaSO", menuName = "Magia/Effetti/EffettoSpawnPiantaSO")]
public class EffettoSpawnPiantaSO : EffettoBaseSO
{
    public int altezzaPianta = 1;
    public Tile testaPianta;
    public Tile corpoPianta;

    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        return;
    }

    public override void ApplicaEffettoANemico(GameObject nemico)
    {
        return;
    }

    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }

    public override IEnumerator TogliEffettoDopoDelTempoANemico(GameObject nemico)
    {
        yield return null;
    }
}
