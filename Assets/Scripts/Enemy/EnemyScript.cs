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
    public Rigidbody2D rigidBody { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }
    
    public float angle { get; set; }
    public float fovAngle { get; set; }
    public float viewDistance { get; set; }

    
    [field: Space(10)]
    [field: Header("Valori Patroling")]
    [field: Space(10)]
    [field: SerializeField] public float patrol_Speed { get; set; }
    [field: SerializeField] public float patrol_angularSpeed { get; set; }
    [field: SerializeField] public float patrol_arriveDistance { get; set; }
    // [field: SerializeField] public List<GameObject> patrol_wayPoints = new List<GameObject>();

    [field: Space(10)]
    [field: Header("Valori Seeking")]
    [field: Space(10)]
    [field: SerializeField, Tooltip("Velocità di inseguimento player")] public float speed { get; set; }
    [field: SerializeField] public float seek_angularSpeed { get; set; }
    [field: SerializeField] public float seek_arriveDistance { get; set; }

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
    public float weaknessMultiplier = 1.5f;
    
    public TipoMagia resistance;

    private GameObject ElementWeaknessObj;
    
    const string ElementWeakness = "ElementWeakness";


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
        
        foreach (Transform child in this.transform)
        {
            if (child.name == ElementWeakness)
            {
                ElementWeaknessObj = child.gameObject;
            }
        }

        // agent.stoppingDistance = behaviorTree.GetVariable()
    }

    public void Start()
    {
        //INIT VARIABLES
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // canPatrol = true;
        ReduceSeeRange();
    }

    public void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // Debug.Log(behaviorTree.GetBehaviorSource().);
        
        FlipSprite();
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

        if (!ElementWeaknessObj.gameObject.activeInHierarchy)
        {
            ElementWeaknessObj.SetActive(true);
        }

        if(magicType == weakness)
        {
            damageCalculated = damageToTake * weaknessMultiplier;
        }
        else
        {
            damageCalculated = damageToTake * 0;
        }
            
        hp -= damageCalculated;

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FlipSprite()
    {
        if(agent.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(agent.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}