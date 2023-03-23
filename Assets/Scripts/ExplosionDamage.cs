using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float ExplosionKnockbackForce = 1;
    public LayerMask DamageContact;
    public int damageExplosion = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DamageContact.Contains(collision.gameObject.layer))
        {
            if(collision.GetComponent<Rigidbody2D>() != null)
            {
                collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - gameObject.transform.position).normalized * ExplosionKnockbackForce*10);
            }
        }
        if (collision.gameObject.GetComponent<EnemyScript>()!=null)
        {
            var enemy = collision.gameObject.GetComponent<EnemyScript>();
            enemy.TakeDamage(damageExplosion);
        }
    }

    public void setLayer(LayerMask layers)
    {
        DamageContact = layers;
    }
}
