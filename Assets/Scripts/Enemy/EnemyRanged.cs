using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    public bool attackCooldown = false;
    public float timeCoolDown = 1.2f;
    public GameObject bullet;
    Coroutine attackCoroutine;

    SpriteRenderer sr;

    public float force = 200f;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    public float attackRange = 18.2f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        FlipSpriteBasedOnPlayerPosition(PlayerCharacterController.Instance.transform);
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Ranged-Enemy-Attack") == false)
        {
            SetMovementAnimation();
        }
    }

    public void AttackEvent()
    {       
        if (!attackCooldown)
        {
            //Debug.Log("ATTACK");

            attackCooldown = true;
            
            _animator.ResetTrigger("Moving");
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Attack");
        }
    }

    public void Shoot()
    {
        // Debug.Log("SHOOT");
        var bulletInst = GameObject.Instantiate(bullet, transform.position, Quaternion.Euler(-180, 0, 0));
        bulletInst.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(PlayerCharacterController.Instance.transform.position.x - transform.position.x, 
                        PlayerCharacterController.Instance.transform.position.y - transform.position.y).normalized * force, 
                        ForceMode2D.Force);
    }

    public void FlipSpriteBasedOnPlayerPosition(Transform playerTransform)
    {
        if (_navMeshAgent.velocity.x == 0)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }
    
    private void SetMovementAnimation()
    {
        if (_navMeshAgent.velocity.magnitude != 0f)
        {
            _animator.SetTrigger("Moving");
        }
        else
        {
            _animator.SetTrigger("Idle");
        }
    }

}
