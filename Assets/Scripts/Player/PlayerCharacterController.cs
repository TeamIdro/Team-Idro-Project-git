using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public static PlayerFacing playerFacingDirection;

    [Header("Player type")]
    [SerializeField] EPlayerType playerType;
    [Space(10)]
    //VARIABILI IN INSPECTOR
    [Header("Player values")]
    [SerializeField] private float movementVelocity;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float maxVelocityCap;
    [Range(0f, 5f)]
    [SerializeField] private float linearDrag;
    [Space(10)]
    [Header("Debug values")]
    [SerializeField,ReadOnly] private Vector2 movementDirection;
    [SerializeField, ReadOnly] private bool isMoving;
    [SerializeField,ReadOnly] private bool isJumping;
    [Space(10)]
    [Header("For raycasting")]
    [SerializeField] private Transform rayCastPosition;
    [SerializeField] private Vector2 boxCastDimension;
    [SerializeField] private LayerMask playerMask;
    

    //VARIABILI PRIVATE
    private Collider2D m_playerMageCollider;
    private Rigidbody2D m_playerMageRB2D;
    private GamePlayInputActions m_gamePlayInputActions;
   

    //PROPRIETA
    public float MageVelocity { get { return movementVelocity; } set { movementVelocity = value; } }
    public Vector2 MovementDirection { get { return movementDirection; } set { movementDirection = value; } }

    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsMoving { get=> IsMoving; set => IsMoving = value; }

    private void Awake()
    {
        m_playerMageRB2D = GetComponent<Rigidbody2D>();
        m_playerMageCollider = GetComponent<Collider2D>();
        m_gamePlayInputActions = new();
    }

    private void Update()
    {
        GetInputDirection();
    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private void GetInputDirection()
    {
        movementDirection.x = m_gamePlayInputActions.Mage.Movimento.ReadValue<Vector2>().x;
        movementDirection.y = m_gamePlayInputActions.Mage.Jump.ReadValue<float>();
       
        if (movementDirection.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (movementDirection.x > 0)
        {
            playerFacingDirection = PlayerFacing.Destra;
        }
        else if (movementDirection.x < 0)
        {
            playerFacingDirection= PlayerFacing.Sinistra;
        }

    }
    private void Movement()
    {
        Vector2 movement = new Vector2(movementDirection.x * movementVelocity * Time.deltaTime,0);
        Debug.Log(movement);
        m_playerMageRB2D.AddForce(movement, ForceMode2D.Impulse);
        if (m_playerMageRB2D.velocity.x > maxVelocityCap || m_playerMageRB2D.velocity.x < -maxVelocityCap)
        {
            m_playerMageRB2D.velocity = new Vector2(Mathf.Clamp(m_playerMageRB2D.velocity.x, -maxVelocityCap, maxVelocityCap),m_playerMageRB2D.velocity.y);
        }
    }
    private void Jump()
    {
        Vector2 jump = new Vector2(0, movementDirection.y);
        if (CheckIfCanJump() && playerType == EPlayerType.Mago)
        {
            IsJumping = !CheckIfCanJump();
            Vector2 jumpWithVelocity = jump.normalized
                * Time.fixedDeltaTime
                * jumpVelocity;
            m_playerMageRB2D.AddForce(Vector2.up * jumpWithVelocity,ForceMode2D.Impulse);
        }


    }

    private bool CheckIfCanJump()
    {
        bool playerCanJump = false;
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(rayCastPosition.position, boxCastDimension, 0, Vector2.down, 0,playerMask);
        if (hit.collider != null)
        {
            playerCanJump = true;
            //Setto il linear drag a 2.5
            m_playerMageRB2D.drag = linearDrag;
        }
        else
        {
            playerCanJump = false;
            m_playerMageRB2D.drag =0;
        }
        return playerCanJump;
    }
    public void ChangePlayerType(EPlayerType type)
    {
        if (type != playerType)
            playerType = type;
        else
            return;
    }
    private void OnEnable()
    {
        m_gamePlayInputActions.Mage.Enable();
    }
  

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(rayCastPosition.position, boxCastDimension);
    }
}
public enum PlayerFacing
{
    Sinistra,
    Destra,
}
