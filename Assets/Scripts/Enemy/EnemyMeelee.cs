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

    public bool attackCooldown;
    public float timeCoolDown = 1.2f;

    // public float attackCooldownSet = 1.2f;

    public GameObject AttackBody;

    Coroutine attackCoroutine;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Terreno");

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, rayGroundRadius, Vector2.down, rayGroundLenght, layerMask);

        if(raycastHit.collider != null)
        {
            isOnGround = OnGround.Ground;
            this.GetComponent<BehaviorTree>().EnableBehavior();
        }
        else if(raycastHit.collider == null)
        {
            isOnGround = OnGround.Air;
            this.GetComponent<BehaviorTree>().DisableBehavior();
        }
    }

    public void Jump()
    {
        if(isOnGround == OnGround.Ground)
        {
            Debug.Log("JUMP");
            rigidbody.AddForce((Vector2.up + (Vector2)transform.forward) * jumpForce);
        }
    }

    public void AttackEvent()//GameObject player
    {       
        if (!attackCooldown)
        {
            Debug.Log("ATTACK");
            AttackBody.SetActive(true);
            attackCooldown = true;
            attackCoroutine = StartCoroutine(CooldownAttack());
        }
    }

    private IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(timeCoolDown);
        attackCooldown = false;
        StopCoroutine(attackCoroutine);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(this.transform.position + (Vector3.down * rayGroundLenght), rayGroundRadius);
    }
}
