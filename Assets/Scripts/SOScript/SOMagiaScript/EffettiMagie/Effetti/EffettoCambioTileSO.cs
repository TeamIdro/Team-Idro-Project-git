using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoCambioTileSO", menuName = "Magia/Effetti/EffettoCambioTileSO")]
public class EffettoCambioTileSO : EffettoBaseSO
{
    public TileChange[] tileChanges;
    public LayerMask targetLayerMask;
    public Vector2Int tileChangeArea;
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


    [System.Serializable]
    public class TileChange
    {
        public TileBase originalTile;
        public TileBase replacementTile;
    }
}
