using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SemePerPianta : MonoBehaviour
{
    public Tilemap tileMapScale;
    [Range(0f, 100f)]
    public int altezzaPianta;

    private Tilemap tileMapSeme;
    private void Awake()
    {
        tileMapSeme= GetComponent<Tilemap>();
    }
    private void OnTriggerEnter2D(Collider2D collision) => StartCoroutine(CostruisciScala(collision));

    private IEnumerator CostruisciScala(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Magia>() != null)
        {
            MagiaSO magiaSo = collision.gameObject.GetComponent<Magia>().magia;
            if (magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoSpawnPiantaSO) as EffettoSpawnPiantaSO != null)
            {
                EffettoSpawnPiantaSO effettoSpawnPianta = magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoSpawnPiantaSO) as EffettoSpawnPiantaSO;
                Vector3 collisionWorldPosition = collision.gameObject.transform.position;
                Vector3Int startPosition = tileMapScale.WorldToCell(collisionWorldPosition);
                Vector3Int basePosition = startPosition + Vector3Int.down;
                tileMapSeme.SetTile(basePosition, effettoSpawnPianta.basePianta);
                for (int i = 0; i < altezzaPianta; i++)
                {
                    if (i == altezzaPianta - 1)
                    {
                        Vector3Int tilePosition = new Vector3Int(basePosition.x, basePosition.y + i, basePosition.z);
                        tileMapScale.SetTile(tilePosition, effettoSpawnPianta.testaPianta);
                    }
                    else
                    {
                        Vector3Int tilePosition = new Vector3Int(basePosition.x, basePosition.y + i, basePosition.z);
                        tileMapScale.SetTile(tilePosition, effettoSpawnPianta.corpoPianta);
                    }
                    yield return new WaitForSeconds(0.2f);
                }
                Destroy(this);
            }
        }
    }


    //private bool IsTileInLayerMask(Vector3Int startPosition)
    //{
    //    Debug.Log("è NELLA LAYER MASK?");
    //    Vector3 worldPosition = tileMapScale.GetCellCenterWorld(startPosition);
    //    Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition, layerMaskDiQuestoOggetto);
    //    return hitCollider != null;
    //}
}
