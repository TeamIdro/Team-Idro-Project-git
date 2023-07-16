using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Magia : MonoBehaviour
{
    [HideInInspector]public LayerMask damageMask;
    [HideInInspector]public MagiaSO magia;
    // Start is called before the first frame update
    [HideInInspector]public float explosionKnockbackForce = 1;
    [HideInInspector]public int damageExplosion = 0;

    [HideInInspector]public GameObject ExplosionPref;
    [HideInInspector]public int damage;
    [HideInInspector]public float decelerationTime = 2f; // tempo in secondi per rallentare completamente il proiettile
    private Rigidbody2D bulletRigidbody;

    public LayerMask ignoreContact;


    IDamageable damageable1;
    private float ticks;
    Vector3 position;
    public float MaxDistance = 100;
    int isDone = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        position = transform.position;
    }
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bulletRigidbody.gravityScale = magia.forzaDiGravitaPerProiettile;
        bulletRigidbody.freezeRotation = true;
    }
    void Update()
    {
        DestroyAfterDistance();
    }
    void FixedUpdate()
    {
        BulletDeceleration();
        Vector2 direction = bulletRigidbody.velocity.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void BulletDeceleration()
    {
        if (bulletRigidbody.velocity.magnitude > 0f)
        {
            if(decelerationTime != 0)
            {
                float decelerationRate = bulletRigidbody.velocity.magnitude / decelerationTime * 2;
                Vector2 oppositeForce = -bulletRigidbody.velocity.normalized * decelerationRate;
                bulletRigidbody.AddForce(oppositeForce, ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        CollisionsBehaviours(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CollisionsBehaviours(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDone = 0;
        CancelInvoke("DamageOrHealCouroutine");
    }

    private void OnDestroy()
    {
        SpawnExplosionParticle();
    }

    private void SpawnExplosionParticle()
    {
        if(ExplosionPref != null)
        {
            
            GameObject expl = Instantiate(ExplosionPref, gameObject.transform.position, gameObject.transform.rotation);
            if (expl != null)
            {
                CameraShake.Instance.ShakeCamera(magia.intensity, magia.shakeTime);
                Destroy(expl, 2);
            }
            else
            {
                Debug.LogWarning("Manca il component ExplosionDamage all'explosion prefab");
            }
        }
     
    }

    private void DestroyAfterDistance()
    {
        float distance = Vector3.Distance(position, transform.position);
        if (distance >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }
    public void SetDamageLayer(LayerMask value)
    {
        damageMask = value;
    }
    public void SetIgnoreLayer(LayerMask value)
    {
        ignoreContact = value;
    }
    public void DestroyAfterTime(float MaxTime)
    {
        //CameraShake.Instance.ShakeCamera(5, 0.1f);
        Destroy(gameObject, MaxTime);
    }
    private void CollisionsBehaviours(Collider2D collision)
    {
        //TODO: risolvere questione layer
            Debug.Log(collision.name);
        if (collision.gameObject.GetComponent<EnemyScript>() is not null)
        {
            damageable1 = collision.gameObject.GetComponent<IDamageable>();
            EnemyScript enemyLocal = collision.gameObject.GetComponent<EnemyScript>();
            if (magia == null) { return; }
            else if (LayerMaskExtensions.IsInLayerMask(collision.gameObject, damageMask))
            {
                magia.ApplicaEffetti(enemyLocal);
                magia.TogliEffettiDopoTempo(enemyLocal);
                if (magia.magicBehaviourType is not TipoComportamentoMagia.Stazionaria)
                {
                    Debug.Log("Preso");
                    damageable1.TakeDamage(magia.dannoDellaMagia, magia.tipoMagia);
                }
                else if(magia.magicBehaviourType is TipoComportamentoMagia.Stazionaria && isDone is not 1)
                {
                    isDone++;
                    InvokeRepeating("DamageOrHealCouroutine",0,magia.tickTime);
                }
                
            }
           
        }
        if (LayerMaskExtensions.IsInLayerMask(collision.gameObject, damageMask) && magia.magicBehaviourType is not TipoComportamentoMagia.Stazionaria)
        {
            Debug.Log("COLLISIONE");
            Debug.Log(collision);
            if (collision.gameObject.GetComponent<Rigidbody2D>() is not null 
                && collision.gameObject.GetComponent<EnemyScript>() is not null
                && collision.gameObject.GetComponent<Spawnpoint>() is null 
                && collision.gameObject.GetComponent<ElementalButton>() is null)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(10 * explosionKnockbackForce * (collision.transform.position - gameObject.transform.position).normalized);
            }
        }
        if (!LayerMaskExtensions.IsInLayerMask(collision.gameObject, ignoreContact) && magia.magicBehaviourType is not TipoComportamentoMagia.Stazionaria)
        {
            if (magia.spawnaOggettoAdImpatto != null)
            {
                Instantiate(magia.spawnaOggettoAdImpatto, gameObject.transform.position, Quaternion.identity);
            }
            if(magia.detonazioneAdImpatto == true)
                Destroy(gameObject);

        }
        
    }
    private void DamageOrHealCouroutine()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), magia.raggioArea,magia.layerMaskPerDanneggiaTarget);
        foreach (Collider2D col in hitColliders)
        {
            Debug.Log(col);
            col.gameObject.GetComponent<IDamageable>().TakeDamage(magia.dannoDellaMagia, magia.tipoMagia);
        }
    }

}
