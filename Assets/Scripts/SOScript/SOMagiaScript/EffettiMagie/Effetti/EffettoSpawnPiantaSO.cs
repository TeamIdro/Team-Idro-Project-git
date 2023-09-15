using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoSpawnPiantaSO", menuName = "Magia/Effetti/EffettoSpawnPiantaSO")]
public class EffettoSpawnPiantaSO : EffettoBaseSO
{
    public Tile testaPianta;
    public Tile corpoPianta;
    public Tile basePianta;

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
