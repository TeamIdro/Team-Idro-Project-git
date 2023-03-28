using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}

public class DestroyOnTrigger : MonoBehaviour
{
    public LayerMask ignoreContact;
    public float damage = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!LayerMaskExtensions.IsInLayerMask(collision.gameObject,ignoreContact))
        {
            Destroy(gameObject);
        }
        // if (collision.gameObject.GetComponent<IDamageable>() != null)
        // {
        //     var enemy = collision.gameObject.GetComponent<IDamageable>();
        //     enemy.TakeDamage(damage, );
        // }
    }

    public void SetLayer(LayerMask layers)
    {
        ignoreContact = layers;
    }
}
