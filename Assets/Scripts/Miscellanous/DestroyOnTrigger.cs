using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}

public class DestroyOnTrigger : MonoBehaviour
{
    public LayerMask ignoreContact;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ignoreContact.Contains(collision.gameObject.layer))
        {
            Destroy(gameObject);
        }
    }

    public void setLayer(LayerMask layers)
    {
        ignoreContact = layers;
    }
}
