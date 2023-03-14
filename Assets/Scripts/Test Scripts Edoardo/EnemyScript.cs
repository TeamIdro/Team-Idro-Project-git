using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System;

public class EnemyScript : MonoBehaviour, IEnemy
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

    private void Awake() 
    {
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

        angle = 280f;
        fovAngle = 120f;
        viewDistance = 7.4f;
        // canPatrol = true;
    }

    public void Update()
    {
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

    public void GetDamage()
    {
        Debug.Log("COLPITO");
        hp -= 100;
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
}