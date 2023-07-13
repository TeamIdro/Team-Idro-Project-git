using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeelee : MonoBehaviour
{
    public float jumpForce = 5f;
    public OnGround isOnGround;
    public float rayGroundLenght;
    public float rayGroundRadius;
    Rigidbody2D rigidbody;
    LayerMask layerMask;
    NavMeshAgent navMeshAgent;

    public bool attackCooldown = false;
    // public float timeCoolDown = 1.2f;

    // public float attackCooldownSet = 1.2f;

    public GameObject attackBody;

    private Animator _animator;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Terreno");

        navMeshAgent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
        // SetMovementAnimation();
    }
    
    void Update()
    {
        // RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, rayGroundRadius, Vector2.down, rayGroundLenght, layerMask);

        // if(raycastHit.collider != null)
        // {
        //     isOnGround = OnGround.Ground;
        //     this.GetComponent<BehaviorTree>().EnableBehavior();
        // }
        // else if(raycastHit.collider == null)
        // {
        //     isOnGround = OnGround.Air;
        //     this.GetComponent<BehaviorTree>().DisableBehavior();
        // }
    }
    
    //TODO: settare attivazione AttackBody con animator

    void FixedUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Meelee-Enemy-Attack") == false)
        {
            SetMovementAnimation();
        }
    }

    public void Jump()
    {
        if(isOnGround == OnGround.Ground)
        {
            Debug.Log("JUMP");
            rigidbody.AddForce(((Vector2.up + (Vector2)transform.forward)).normalized * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void AttackEvent()
    {       
        if (!attackCooldown)
        {
            Debug.Log("ATTACK");
            
            attackCooldown = true;
            
            _animator.ResetTrigger("Moving");
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Attack");

        }
        // else
        // {
        //     _animator.ResetTrigger("Attack");
        // }
    }

    // private IEnumerator CooldownAttack()
    // {
    //     yield return new WaitForSeconds(timeCoolDown);
    //     _animator.SetTrigger("Attack");
    //     attackCooldown = false;
    //     StopCoroutine(attackCoroutine);
    // }

    // private void OnDrawGizmos() 
    // {
    //     Gizmos.DrawSphere(this.transform.position + (Vector3.down * rayGroundLenght), rayGroundRadius);
    // }
    
    private void SetMovementAnimation()
    {
        
        if (navMeshAgent.velocity.magnitude != 0f)
        {
            _animator.SetTrigger("Moving");
        }
        else
        {
            _animator.SetTrigger("Idle");
        }
    }
    
    private void ActivateAttackBody()
    {
        attackBody.SetActive(true);
    }
    
    private void DeactivateAttackBody()
    {
        attackBody.SetActive(false);
    }
}
