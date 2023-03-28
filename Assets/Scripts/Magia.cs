using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Magia : MonoBehaviour
{
    public LayerMask damageMask;
    public MagiaSO magia;
    public bool isCasted = false;
    public Animator animator;
    private PlayerFacing playerFacingOnInstance;
    private SpriteRenderer spriteRendererMagia;
    // Start is called before the first frame update
   

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCasted = false;
        //TODO: risolvere questione layer
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            var enemy = collision.gameObject.GetComponent<EnemyScript>();
            if (magia == null) { return; }
            else if (LayerMaskExtensions.IsInLayerMask(collision.gameObject,damageMask))
            {
                Debug.Log("Preso");
                enemy.TakeDamage(magia.dannoDellaMagia, magia.tipoMagia);
            }
        }
    }
    /// <summary>
    /// set the damage layer that needs to check
    /// </summary>
    /// <param name="damageValue"></param>
    public void SetDamageLayer(LayerMask value)
    {
        damageMask = value;
    }

    private void OnDisable()
    {
        magia = null;
    }
}
