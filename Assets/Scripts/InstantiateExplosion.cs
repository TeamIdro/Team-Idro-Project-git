using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateExplosion : MonoBehaviour
{
    public float explosionKnockbackForce = 1;
    public GameObject ExplosionPref;
    public LayerMask DamageContact;
    private void OnDestroy()
    {
        GameObject expl = Instantiate(ExplosionPref, gameObject.transform.position, gameObject.transform.rotation);
        ExplosionDamage ExlDam = expl.GetComponent<ExplosionDamage>();
        if (ExlDam != null)
        {
            ExlDam.setLayer(DamageContact);
            ExlDam.ExplosionKnockbackForce = explosionKnockbackForce;
        }
        else
        {
            Debug.LogWarning("Manca il component ExplosionDamage all'explosion prefab");
        }
        Destroy(expl, 5);
    }
}
