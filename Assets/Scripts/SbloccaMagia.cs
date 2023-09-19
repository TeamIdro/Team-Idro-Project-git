using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SbloccaMagia : MonoBehaviour
{

    [SerializeField] MagiaSO magiaDaSbloccare;
    [SerializeField] string nomeDellaMagia;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MagicController>() != null)
        {
            MagicController controller = collision.GetComponent<MagicController>();
            controller.AggiungiMagiaAllaLista(magiaDaSbloccare,nomeDellaMagia);
            Destroy(gameObject);
        }
    }


}
