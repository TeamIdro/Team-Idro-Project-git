using PubSub;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour, ISubscriber,IDamageable
{
    public static PlayerFacingDirections playerFacingDirections = PlayerFacingDirections.Right;
    
    [Header("Player type")]
    [SerializeField] EPlayerType playerType;
    [Space(10)]
    [Header("GUI")]
    [SerializeField] Slider m_healthSlider;
    [Header("Player values")]
    public float hp;
    [SerializeField] private float movementVelocity;
    [SerializeField] public float jumpVelocity;
    [SerializeField, ReadOnly] private bool isBlocked = false;
    [SerializeField] private float maxVelocityCap;
    [SerializeField] private float deceleration;
    [Range(0f, 5f)]
    [SerializeField] private float maxCastDistance;
    [SerializeField] private float climbSpeed;
    [Header("Player Audios List")]
    [Space(10)]
    [Header("Debug values")]
    [SerializeField,ReadOnly] private Vector2 movementDirection;
    [SerializeField, ReadOnly] private bool isMoving;
    [SerializeField,ReadOnly] private bool isJumping;
    [SerializeField, ReadOnly] private bool isOnStairs = false;

    [Space(10)]
    [Header("For raycasting")]
    [SerializeField] private Transform rayCastPosition;
    [SerializeField] private Vector2 boxCastDimension;
    [SerializeField] private LayerMask playerMask;
    [Header("Unity Event")]
    
   

    //VARIABILI PRIVATE
    private Collider2D m_playerMageCollider;
    private Rigidbody2D m_playerMageRB2D;
    private GamePlayInputActions m_gamePlayInputActions;
    private Animator animatorMago;
    private SpriteRenderer mageRenderer;
    private float guardaSuValue = 0;
    private float guardaGiuValue = 0;
    private bool jumpIsOff;
    private float m_maxHealth;
    float verticalInput = 0;
    //PROPRIETA
    public float MageAcceleration { get { return movementVelocity; } set { movementVelocity = value; } }
    public float MaxVelocityCap { get { return maxVelocityCap; } set { maxVelocityCap = value; } }
    public Vector2 MovementDirection { get { return movementDirection; } set { movementDirection = value; } }

    public bool IsMoving { get=> IsMoving; set => IsMoving = value; }
    public bool IsAscendingForJump { get => IsAscendingForJump; set => IsAscendingForJump = value; }
    public bool IsOnStairs { get => isOnStairs; set => isOnStairs = value; }

    private static PlayerCharacterController _instance;

    public static PlayerCharacterController Instance
    {
        get
        {
            return _instance;
        }
    }

    public PlayerSaveData playerSaveData;

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
        // playerSaveData.WriteData();
        m_healthSlider.maxValue = hp;
        m_maxHealth = hp;
        Publisher.Subscribe(this, new StopOnOpenPauseMessage());
        Publisher.Subscribe(this, new StartOnClosedPauseMessage());

    }

    private void Update()
    {
        GetInputDirection();
        AnimationUpdate();
        (IsOnStairs ? (Action)ClimbStairs : Movement)();
        if (!isOnStairs)
        {
            isJumping = !CheckIfCanJump();
        }
        else
        {
            return;
        }
        if (isBlocked) { return; }
    }

   

    private void FixedUpdate()
    {
        if (isBlocked) { return; }
    }


    private void ClimbStairs()
    {
        isJumping = false;
        float horizontalInput = movementDirection.x;
        if (m_gamePlayInputActions.Mage.GuardaSu.ReadValue<float>()== 1)
            verticalInput = 1;
        else if (m_gamePlayInputActions.Mage.GuardaGiu.ReadValue<float>() == 1)
            verticalInput = -1;
        else { verticalInput = 0; }
        Debug.Log(verticalInput);
        Vector2 climbVelocity = new Vector2(horizontalInput * movementVelocity * Time.fixedDeltaTime, verticalInput * climbSpeed * Time.fixedDeltaTime);
        m_playerMageRB2D.velocity = climbVelocity;
    }

    private void GetInputDirection()
    {
        movementDirection.x = m_gamePlayInputActions.Mage.Movimento.ReadValue<Vector2>().x;
        movementDirection.y = m_gamePlayInputActions.Mage.Jump.ReadValue<float>();
        guardaSuValue = m_gamePlayInputActions.Mage.GuardaSu.ReadValue<float>();
        guardaGiuValue = m_gamePlayInputActions.Mage.GuardaGiu.ReadValue<float>();


        if (movementDirection.x != 0)
            isMoving = true;
        else
            isMoving = false;
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
            playerFacingDirections = PlayerFacingDirections.Right;
        else if(mageRenderer.flipX == true)
            playerFacingDirections = PlayerFacingDirections.Left;


        if (movementDirection.x > 0)
            mageRenderer.flipX = false;
        else if (movementDirection.x < 0)
            mageRenderer.flipX = true;
    }
    private void AnimationUpdate()
    {
        animatorMago.speed = 1;
        animatorMago.SetBool("IsMoving",isMoving);
        animatorMago.SetFloat("YVelocity",Mathf.Floor(m_playerMageRB2D.velocity.y));
        animatorMago.SetBool("IsGrounded", !isJumping);
        animatorMago.SetBool("IsClimbing", IsOnStairs);
        if (isOnStairs)
        {
            if(Mathf.Floor(m_playerMageRB2D.velocity.y) > 0)
                animatorMago.speed = 1;
            else if(Mathf.Floor(m_playerMageRB2D.velocity.y) == 0)
                animatorMago.speed = 0;
            else if(Mathf.Floor(m_playerMageRB2D.velocity.y) < 0)
            {
                Debug.LogAssertion("ANIMATION CLIP AL CONTRARIO");
                animatorMago.speed = -1;
            }
            animatorMago.Play("MageClimbing");
        }
    }
    private void Movement()
    {
        Vector2 movement = new(movementDirection.x * movementVelocity * Time.deltaTime, 0);
        m_playerMageRB2D.AddForce(movement, ForceMode2D.Impulse);
        if (m_playerMageRB2D.velocity.x > maxVelocityCap || m_playerMageRB2D.velocity.x < -maxVelocityCap)
        {
            m_playerMageRB2D.velocity = new Vector2(Mathf.Clamp(m_playerMageRB2D.velocity.x, -maxVelocityCap, maxVelocityCap), m_playerMageRB2D.velocity.y);
        }
        if (Math.Abs(movementDirection.x) < 0.1f)
        {
            m_playerMageRB2D.velocity = Vector2.Lerp(new Vector2(m_playerMageRB2D.velocity.x, m_playerMageRB2D.velocity.y), new Vector2(0, m_playerMageRB2D.velocity.y), deceleration * Time.fixedDeltaTime);
        }

    }
    private void Jump(InputAction.CallbackContext context)
    {
        Vector2 jump = new Vector2(0, 1);
        if (CheckIfCanJump() && playerType == EPlayerType.Mago && !jumpIsOff && !isOnStairs)
        {
            isJumping = !CheckIfCanJump();
            Vector2 jumpWithVelocity = jump
                * Time.fixedDeltaTime
                * jumpVelocity;
            m_playerMageRB2D.AddForce(Vector2.up * jumpWithVelocity,ForceMode2D.Impulse);
            
        }


    }
  
    public void ToggleJump(bool toggle)
    {
        Debug.Log("TOGGLE JUMP");
        Debug.Log(toggle);
        if(!toggle)
            jumpIsOff = true;
        else
            jumpIsOff = false;
    }

    private bool CheckIfCanJump()
    {
        bool playerCanJump = false;
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(rayCastPosition.position, boxCastDimension, 0, Vector2.down, maxCastDistance,playerMask);
        if (hit.collider != null)
            playerCanJump = true;
        else
            playerCanJump = false;
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
        if(hp > 0)
        {
            hp -= damage;
            m_healthSlider.value = hp;
        }
        
        if(hp <= 0)
        {
            SpawnManager.Instance?.Respawn();
            hp = m_maxHealth;
            m_healthSlider.value = hp;
        }
    }


    public void OnPublish(IMessage message)
    {
        if(message is StopOnOpenPauseMessage)
        {
            isBlocked = true;
            m_healthSlider.gameObject.SetActive(false);
            animatorMago.enabled = false;
        }
        else if(message is StartOnClosedPauseMessage)
        {
            isBlocked = false;
            m_healthSlider.gameObject.SetActive(true);
            animatorMago.enabled = true;

        }
    }

    public void TakeDamage(float damageToTake,TipoMagia magicType)
    {
        if (hp > 0)
        {
            hp -= damageToTake;
            m_healthSlider.value = hp;
        }

        if (hp <= 0)
        {
            SpawnManager.Instance?.Respawn();
            hp = m_maxHealth;
            m_healthSlider.value = hp;
        }
    }
    public void EnableController()
    {
        m_gamePlayInputActions.Mage.Enable();
    }
    public void DisableControl()
    {
        m_gamePlayInputActions.Mage.Disable();
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

