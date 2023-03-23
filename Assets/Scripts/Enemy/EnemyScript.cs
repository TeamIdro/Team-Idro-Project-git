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
    [field: SerializeField] public int hp { get; set; }
    [field: SerializeField] public int attack { get; set; }
    [field: SerializeField] public float speed { get; set; }
    [field: SerializeField] public Rigidbody2D rigidBody { get; set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; set; }
    
    [field: SerializeField] public float angle { get; set; }
    [field: SerializeField] public float fovAngle { get; set; }
    [field: SerializeField] public float viewDistance { get; set; }
    // [field: SerializeField] public bool canPatrol { get; set; }

    private NavMeshAgent agent;
    private BehaviorTree behaviorTree;

    // public float attackCooldown = 0f;
    // public float attackCooldownSet = 5f;

    [Space(10)]
    [Header("Variabili Valori Inseguimento Player")]
    [Header("SetReduce... sono quelli di partenza")]
    public float setReducedAngle = 280f;
    public float setReducedFovAngle = 120f;
    public float setReducedViewDistance = 7.4f;

     public float setIncreasedAngle = 280f;
    public float setIncreasedFovAngle = 360f;
    public float setIncreasedViewDistance = 10f;

    public TipoMagia weakness; 

    private void Awake() 
    {
        angle = setReducedAngle;
        fovAngle = setReducedAngle;
        viewDistance = setReducedAngle;

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
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
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

    public void TakeDamage(int damageToTake, TipoMagia magicType)
    {
        
        if (hp > 0)
        {
            Debug.Log("Prendo danno");
            Debug.Log(damageToTake);
            hp = hp - damageToTake;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        // Debug.Log("MORTO" + this.gameObject.name);
    }
}