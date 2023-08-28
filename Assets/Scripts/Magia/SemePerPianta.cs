using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SemePerPianta : MonoBehaviour
{
    public Tilemap tileMapScale;
    [Range(0f, 100f)]
    public int altezzaPianta;
    private void OnTriggerEnter2D(Collider2D collision) => StartCoroutine(CostruisciScala(collision));

    private IEnumerator CostruisciScala(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Magia>() != null)
        {
            MagiaSO magiaSo = collision.gameObject.GetComponent<Magia>().magia;
            if (magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoSpawnPiantaSO) as EffettoSpawnPiantaSO != null)
            {
                EffettoSpawnPiantaSO effettoSpawnPianta = magiaSo.effettiMagiaQuandoColpito.Find(x => x as EffettoSpawnPiantaSO) as EffettoSpawnPiantaSO;
                Vector3Int positionOfStart = tileMapScale.WorldToCell(collision.gameObject.transform.position);

                for (int i = 0; i < altezzaPianta; i++)
                {
                    Vector3Int tilePosition = new Vector3Int(positionOfStart.x, positionOfStart.y + i, positionOfStart.z);
                    tileMapScale.SetTile(tilePosition, effettoSpawnPianta.corpoPianta);
                    yield return new WaitForSeconds(0.2f);
                }
            }

        }
    }
    //private bool IsTileInLayerMask(Vector3Int tilePosition)
    //{
    //    Debug.Log("è NELLA LAYER MASK?");
    //    Vector3 worldPosition = tileMapScale.GetCellCenterWorld(tilePosition);
    //    Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition, layerMaskDiQuestoOggetto);
    //    return hitCollider != null;
    //}
}
