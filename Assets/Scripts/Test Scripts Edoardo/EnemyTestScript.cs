using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System;

public class EnemyTestScript : MonoBehaviour, IEnemy
{
    [field: SerializeField] public EnemyCategory category { get; set; }
    [field: SerializeField] public Weakness weakness { get; set; }
    [field: SerializeField] public int hp { get; set; }
    [field: SerializeField] public int attack { get; set; }
    [field: SerializeField] public float speed { get; set; }
    [field: SerializeField] public Rigidbody2D rigidBody { get; set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; set; }
    
    [field: SerializeField] public float angle { get; set; }
    [field: SerializeField] public bool canPatrol { get; set; }

    private NavMeshAgent agent;
    private BehaviorTree behaviorTree;

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
        // canPatrol = true;
    }

    public void Update()
    {
        behaviorTree.RegisterEvent<object>("Attack", AttackEvent);
        // behaviorTree.RegisterEvent<object>("SetCanPatrol", SetCanPatrolEvent);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void AttackEvent(object arg1)
    {
        // Debug.Log("ARG: " + arg1);
        var behaviorTree = GetComponent<BehaviorTree>();
    }

    private void SetCanPatrolEvent(object arg1)
    {

    }
}