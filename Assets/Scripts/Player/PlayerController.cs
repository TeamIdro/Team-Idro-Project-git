using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Stats")]
    public float playerAcceleration;
    public float maxPlayerVelocity;
    public float directionOffset;

    
    private Rigidbody2D m_rigidbody;
    private Collider2D m_collider;
    private Animator m_animator;    

    private Vector2 m_inputDirection;

    private PlayerInput m_actions;
    private InputAction m_movementAction;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_animator = GetComponentInChildren<Animator>();

        m_actions = GetComponent<PlayerInput>();
        m_movementAction = m_actions.actions["Movement"];
    }

    private void Update()
    {
        UpdateInput();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Move the player based on the input direction
    /// </summary>
    private void Movement()
    {
        Vector2 clampedInputDirection = Vector2.ClampMagnitude(m_inputDirection, 1);
        if (m_rigidbody.velocity.magnitude > maxPlayerVelocity)
            m_rigidbody.velocity = Vector2.ClampMagnitude(m_rigidbody.velocity,maxPlayerVelocity);
        m_rigidbody.AddForce(clampedInputDirection * playerAcceleration * Time.deltaTime);

    }
    /// <summary>
    /// Update the input
    /// </summary>
    private void UpdateInput()
    {
        m_inputDirection = m_movementAction.ReadValue<Vector2>();
        //m_animator.SetFloat("MovementX", m_inputDirection.x);
        //m_animator.SetFloat("MovementY", m_inputDirection.y);
    }
}
