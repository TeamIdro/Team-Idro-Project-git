using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallentaProiettile : MonoBehaviour
{
    public float decelerationTime = 2f; // tempo in secondi per rallentare completamente il proiettile
    private Rigidbody2D bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (bulletRigidbody.velocity.magnitude > 0f)
        {
            float decelerationRate = bulletRigidbody.velocity.magnitude / decelerationTime*2;
            Vector2 oppositeForce = -bulletRigidbody.velocity.normalized * decelerationRate;
            bulletRigidbody.AddForce(oppositeForce, ForceMode2D.Force);
        }
    }
}
