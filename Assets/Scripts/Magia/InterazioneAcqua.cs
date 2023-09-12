using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class InterazioneAcqua : MonoBehaviour
{
    private Tilemap tilemapSuCuiSiPosaQuestoOggetto;
    public Tilemap tileSuCuiDisegnareBlocchi;
    [SerializeField] private LayerMask layerMaskDiQuestoOggetto;

    private void Awake()
    {
        tilemapSuCuiSiPosaQuestoOggetto = gameObject.GetComponent<Tilemap>();
    }

        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Magia>() != null)
        {
            MagiaSO magiaSo =  collision.gameObject.GetComponent<Magia>().magia;
            if(magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoCambioTileSO) as EffettoCambioTileSO != null)
            {
                EffettoCambioTileSO effettoCambioTile = magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoCambioTileSO) as EffettoCambioTileSO;
                foreach (var tileChange in effettoCambioTile.tileChanges)
                {
                    Vector3Int hitTilePosition = tilemapSuCuiSiPosaQuestoOggetto.WorldToCell(collision.transform.position);
                    BoundsInt bounds = new BoundsInt(
                        hitTilePosition.x, hitTilePosition.y-1, 0,
                        effettoCambioTile.tileChangeArea.x, effettoCambioTile.tileChangeArea.y, 1);
                    TileBase[] allTiles = tilemapSuCuiSiPosaQuestoOggetto.GetTilesBlock(bounds);

                    for (int x = 0; x < bounds.size.x; x++)
                    {
                        for (int y = 0; y < bounds.size.y; y++)
                        {
                            TileBase tile = allTiles[x + y * bounds.size.x];
                            if(tile != null)
                            {
                                Debug.ClearDeveloperConsole();
                                Debug.Log("FACCIO UN CHECK AL TILE "+ tile);
                                if (tile == tileChange.originalTile)
                                {
                                    Vector3Int pos = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                                    Debug.Log("ENTRO PER SETTARE I TILE ALL POS: "+pos);
                                    tileSuCuiDisegnareBlocchi.SetTile(pos, tileChange.replacementTile);

                                }

                            }
                        }
                    }
                }
                tileSuCuiDisegnareBlocchi.gameObject.layer = LayerMask.NameToLayer("Terreno");
            }
        }
        else if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(1000000, TipoMagia.Acqua);
        }
    }

    private bool IsTileInLayerMask(Vector3Int tilePosition)
    {
        Debug.Log("è NELLA LAYER MASK?");
        Vector3 worldPosition = tilemapSuCuiSiPosaQuestoOggetto.GetCellCenterWorld(tilePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition, layerMaskDiQuestoOggetto);
        return hitCollider != null;
    }
}
