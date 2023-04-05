using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    public static PlayerFacingDirections playerFacingDirections = PlayerFacingDirections.Right;

    [Header("Player type")]
    [SerializeField] EPlayerType playerType;
    [Space(10)]
    //VARIABILI IN INSPECTOR
    [Header("Player values")]
    public float hp;
    [SerializeField] private float movementVelocity;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float maxVelocityCap;
    [SerializeField] private float deceleration;
    [Range(0f, 5f)]
    [SerializeField] private float maxCastDistance;
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
    private Animator animatorMago;
    private SpriteRenderer mageRenderer;
    private float guardaSuValue = 0;
    private float guardaGiuValue = 0;

    //PROPRIETA
    public float MageVelocity { get { return movementVelocity; } set { movementVelocity = value; } }
    public Vector2 MovementDirection { get { return movementDirection; } set { movementDirection = value; } }

    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsMoving { get=> IsMoving; set => IsMoving = value; }

    private static PlayerCharacterController _instance;

    public static PlayerCharacterController Instance
    {
        get
        {
            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
        m_playerMageRB2D = GetComponent<Rigidbody2D>();
        m_playerMageCollider = GetComponent<Collider2D>();
        m_gamePlayInputActions = new();
        animatorMago = GetComponentInChildren<Animator>();
        mageRenderer = GetComponentInChildren<SpriteRenderer>();
        m_gamePlayInputActions.Mage.Jump.started += Jump;
    }
    private void Start()
    {
       

    }

    private void Update()
    {
        GetInputDirection();
        AnimationUpdate();
    }

   

    private void FixedUpdate()
    {
        Movement();
    }

    private void GetInputDirection()
    {
        movementDirection.x = m_gamePlayInputActions.Mage.Movimento.ReadValue<Vector2>().x;
        movementDirection.y = m_gamePlayInputActions.Mage.Jump.ReadValue<float>();
        guardaSuValue = m_gamePlayInputActions.Mage.GuardaSu.ReadValue<float>();
        guardaGiuValue = m_gamePlayInputActions.Mage.GuardaGiu.ReadValue<float>();

        Debug.Log("il valore del flip x è: "+mageRenderer.flipX +" mentre il valore della facing direction è: "+ PlayerCharacterController.playerFacingDirections.ToString());
        Debug.Log("il valore di guarda su è: " + guardaSuValue + " mentre guarda giu è: " + guardaGiuValue);

        if (movementDirection.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (guardaSuValue != 0)
        {
            playerFacingDirections = PlayerFacingDirections.Up;
            guardaSuValue = 0;
            return;
        }
        else if (guardaGiuValue != 0)
        {
            playerFacingDirections = PlayerFacingDirections.Down;
            guardaGiuValue = 0;
            return;
        }
        else if (mageRenderer.flipX == false)
        {
            playerFacingDirections = PlayerFacingDirections.Right;
        }
        else if(mageRenderer.flipX == true)
        {
            playerFacingDirections = PlayerFacingDirections.Left;
        }


       
        
     
        //else
        //{
        //    playerFacingDirections = PlayerFacingDirections.ZeroForLookUpandDown;
        //}
        if (movementDirection.x > 0)
        {
            mageRenderer.flipX = false;
        }
        else if (movementDirection.x < 0)
        {
            mageRenderer.flipX = true;
        }
    }
    private void AnimationUpdate()
    {
        animatorMago.SetBool("IsMoving",isMoving);
        animatorMago.SetBool("isJumping",isJumping);

    }
    private void Movement()
    {
        Vector2 movement = new Vector2(movementDirection.x * movementVelocity * Time.deltaTime,0);
        m_playerMageRB2D.AddForce(movement, ForceMode2D.Impulse);
        if (m_playerMageRB2D.velocity.x > maxVelocityCap || m_playerMageRB2D.velocity.x < -maxVelocityCap)
        {
            m_playerMageRB2D.velocity = new Vector2(Mathf.Clamp(m_playerMageRB2D.velocity.x, -maxVelocityCap, maxVelocityCap),m_playerMageRB2D.velocity.y);
        }
        if (Math.Abs(movementDirection.x) < 0.1f)
        {
            m_playerMageRB2D.velocity = Vector2.Lerp(new Vector2(m_playerMageRB2D.velocity.x, m_playerMageRB2D.velocity.y), new Vector2(0, m_playerMageRB2D.velocity.y), deceleration * Time.fixedDeltaTime);
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        var inputJump = context.ReadValue<float>();
        Vector2 jump = new Vector2(0, 1);
        if (CheckIfCanJump() && playerType == EPlayerType.Mago)
        {
            isJumping = !CheckIfCanJump();
            Vector2 jumpWithVelocity = jump
                * Time.fixedDeltaTime
                * jumpVelocity;
            m_playerMageRB2D.AddForce(Vector2.up * jumpWithVelocity,ForceMode2D.Impulse);
        }


    }

    private bool CheckIfCanJump()
    {
        bool playerCanJump = false;
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(rayCastPosition.position, boxCastDimension, 0, Vector2.down, maxCastDistance,playerMask);
        if (hit.collider != null)
        {
            playerCanJump = true;
            //Setto il linear drag a 2.5
        }
        else
        {
            playerCanJump = false;
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

    public void GetDamage(float damage)
    {
        hp -= damage;

        if(hp < 0)
        {
            SpawnManager.Instance.Respawn();
            hp = 50f;
        }
    }
}
public enum PlayerFacingDirections
{
    Left,
    Right,
    Up,
    Down,
    ZeroForLookUpandDown,
}

