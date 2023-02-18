using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayer : MonoBehaviour
{
    public Vector2 movementVector;
    public void Start()
    {
    
    }

    public void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        var rb = GetComponent<Rigidbody2D>();
        Vector2 actualPosition = new Vector2(transform.position.x, transform.position.y);

        rb.MovePosition(Vector2.Lerp(actualPosition, actualPosition + movementVector, 1f));
    }

    public void OnMove(InputValue value)
    {
        
        movementVector = value.Get<Vector2>();
        
    }
}
