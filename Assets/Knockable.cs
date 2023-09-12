using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockable : MonoBehaviour
{
    public float knockbackSpeed = 5.0f; // La velocità del knockback
    public float knockbackDistance = 100.0f; // La distanza del knockback desiderata

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool isKnockingBack = false;
    private Vector2 directionOfKnockBack;
    private float distanceKnocked = 0.0f; // Distanza percorsa durante il knockback

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isKnockingBack)
        {
            float moveDistance = knockbackSpeed * Time.deltaTime;
            Vector2 newPosition;
            if (directionOfKnockBack == Vector2.right)
            {
                newPosition = new Vector2(transform.position.x + moveDistance, transform.position.y);
                rb.MovePosition(newPosition);
            }
            else if (directionOfKnockBack == Vector2.left)
            {
                newPosition = new Vector2(transform.position.x - moveDistance, transform.position.y);
                rb.MovePosition(newPosition);
            }
            distanceKnocked += moveDistance;
            if (distanceKnocked >= knockbackDistance)
            {
                isKnockingBack = false;
                rb.velocity = Vector2.zero; 
            }
        }
    }

    public void ActivateKnockback(Vector2 direction)
    {
        directionOfKnockBack = direction;
        isKnockingBack = true;
        initialPosition = transform.position;
        distanceKnocked = 0.0f;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 3, 0);
    }
}
