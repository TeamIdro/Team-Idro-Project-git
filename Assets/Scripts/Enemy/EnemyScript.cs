using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using System;

public enum OnGround {Ground, Air}
public class EnemyScript : MonoBehaviour, IEnemy, IDamageable
{
    // [field: SerializeField] public EnemyCategory category { get; set; }
    // [field: SerializeField] public Weakness weakness { get; set; }
    [field: SerializeField] public float hp { get; set; }
    public int attack { get; set; }
    public float speed { get; set; }
    public Rigidbody2D rigidBody { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }
    
    public float angle { get; set; }
    public float fovAngle { get; set; }
    public float viewDistance { get; set; }

    private NavMeshAgent agent;
    private BehaviorTree behaviorTree;

    [Space(10)]
    [Header("Valori inseguimento player")]
    [Header("SetReduce sono quelli di partenza")]
    [Space(10)]

    public float setReducedAngle = 280f;
    public float setReducedFovAngle = 120f;
    public float setReducedViewDistance = 7.4f;

    public float setIncreasedAngle = 280f;
    public float setIncreasedFovAngle = 360f;
    public float setIncreasedViewDistance = 10f;
    
    [field: SerializeField]
    public float attackDistance { get; set; }

    [Space(10)]
    [Header("Weakness and Resistance")]
    public TipoMagia weakness; 
    public TipoMagia resistance; 

    private void Awake() 
    {
        angle = setReducedAngle;
        fovAngle = setReducedAngle;
        viewDistance = setReducedAngle;

        agent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorTree>();    
            
        agent.updateRotation = false;
		agent.updateUpAxis = false;

        ReduceSeeRange();

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
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // Debug.Log(behaviorTree.GetBehaviorSource().);
    }

    
    public void ReduceSeeRange()
    {
        angle = setReducedAngle;
        fovAngle = setReducedFovAngle;
        viewDistance = setReducedViewDistance;
    }

    public void IncreaseSeeRange()
    {
        angle = setIncreasedAngle;
        fovAngle = setIncreasedFovAngle;
        viewDistance = setIncreasedViewDistance;
    }

    public void TakeDamage(float damageToTake, TipoMagia magicType)
    {
        float damageCalculated = damageToTake;

        if(magicType == weakness)
        {
            damageCalculated = damageToTake * 1.5f;
        }
        else if(magicType == resistance)
        {
            damageCalculated = damageToTake * 0.5f;
        }
            
        hp -= damageCalculated;

        if (hp < 0)
        {
            Destroy(this.gameObject);
        }

    }
}