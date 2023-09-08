using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockable : MonoBehaviour
{
    public float knockbackSpeed = 5.0f; // La velocità del knockback
    public float knockbackDistance = 3.0f; // La distanza del knockback

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool isKnockingBack = false;
    private Vector2 directionOfKnockBack;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isKnockingBack)
        {
            // Calcola la nuova posizione in base alla velocità
            float moveDistance = knockbackSpeed * Time.deltaTime;
            Vector2 newPosition;
            if (directionOfKnockBack == Vector2.right)
            {
                newPosition = new Vector2(transform.position.x + moveDistance, transform.position.y);
                rb.MovePosition(newPosition);
            }
            else if(directionOfKnockBack == Vector2.left)
            {
                newPosition = new Vector2(transform.position.x - moveDistance, transform.position.y);
                rb.MovePosition(newPosition);
            }

            // Sposta l'oggetto

            // Verifica se il knockback è completo
            if (Mathf.Abs(transform.position.x - initialPosition.x) >= knockbackDistance)
            {
                isKnockingBack = false;
                rb.velocity = Vector2.zero; // Arresta l'oggetto quando il knockback è completo
            }
        }
    }

    // Attiva l'effetto di knockback
    public void ActivateKnockback(Vector2 direction)
    {
        directionOfKnockBack = direction;
        isKnockingBack = true;
        initialPosition = transform.position;
    }
}
