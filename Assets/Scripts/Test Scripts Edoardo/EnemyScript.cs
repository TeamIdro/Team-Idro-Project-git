using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System;

public enum OnGround {Ground, Air}
public class EnemyScript : MonoBehaviour, IEnemy, IDamageable
{
    [field: SerializeField] public EnemyCategory category { get; set; }
    [field: SerializeField] public Weakness weakness { get; set; }
    [field: SerializeField] public int hp { get; set; }
    [field: SerializeField] public int attack { get; set; }
    [field: SerializeField] public float speed { get; set; }
    [field: SerializeField] public Rigidbody2D rigidBody { get; set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; set; }
    
    [field: SerializeField] public float angle { get; set; }
    [field: SerializeField] public float fovAngle { get; set; }
    [field: SerializeField] public float viewDistance { get; set; }
    [field: SerializeField] public bool canPatrol { get; set; }

    private NavMeshAgent agent;
    private BehaviorTree behaviorTree;

    public float attackCooldown = 0f;
    public float attackCooldownSet = 5f;
    public float jumpForce = 5f;

    public OnGround isOnGround;
    public float rayGroundLenght;

    private void Awake() 
    {
        angle = 280f;
        fovAngle = 120f;
        viewDistance = 7.4f;

        agent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorTree>();

		agent.updateRotation = false;
		agent.updateUpAxis = false;
        // agent.stoppingDistance = behaviorTree.GetVariable()
    }

    public void Start()
    {
        //INIT VARIABLES
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // canPatrol = true;
    }

    public void Update()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, (transform.lossyScale.y) + rayGroundLenght);
        Debug.Log(raycastHit.collider.name);

        if(raycastHit.collider != null 
            && raycastHit.collider.IsTouchingLayers(9))
        {
            isOnGround = OnGround.Ground;
        }
        else if(raycastHit.collider == null)
        {
            isOnGround = OnGround.Air;
        }

        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void AttackEvent()//GameObject player
    {
         
        if (attackCooldown <= 0f)
        {
            Debug.Log("ATTACK");
            attackCooldown = attackCooldownSet;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
        
    }

  

    public void ReduceSeeRange()
    {
        angle = 280f;
        fovAngle = 120f;
        viewDistance = 7.4f;
    }

    public void IncreaseSeeRange()
    {
        angle = 280f;
        fovAngle = 360f;
        viewDistance = 10f;
    }

    public void Jump()
    {
        if(isOnGround == OnGround.Ground)
        {
            Debug.Log("JUMP");
            rigidBody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(this.transform.position, new Vector3(transform.position.x, (transform.lossyScale.y) + rayGroundLenght, 0f));
    }

    public void TakeDamage(int damageToTake)
    {
        if (hp > 0)
        {
            Debug.Log("Prendo danno");
            Debug.Log(damageToTake);
            hp = hp -damageToTake;
        }
    }
}