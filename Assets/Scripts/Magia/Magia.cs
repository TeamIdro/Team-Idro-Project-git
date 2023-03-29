using UnityEngine;

public class Magia : MonoBehaviour
{
    public LayerMask damageMask;
    public MagiaSO magia;
    public bool isCasted = false;
    public Animator animator;
    private PlayerFacing playerFacingOnInstance;
    private SpriteRenderer spriteRendererMagia;
    // Start is called before the first frame update
    public float ExplosionKnockbackForce = 1;
    public int damageExplosion = 0;

    public GameObject ExplosionPref;
    public int damage;

    public float decelerationTime = 2f; // tempo in secondi per rallentare completamente il proiettile
    private Rigidbody2D bulletRigidbody;

    public LayerMask ignoreContact;


    Vector3 position;
    public float MaxDistance;
    // Start is called before the first frame update
    private void Awake()
    {
        position = transform.position;
    }
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float distance = Vector3.Distance(position, transform.position);
        if (distance >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (bulletRigidbody.velocity.magnitude > 0f)
        {
            float decelerationRate = bulletRigidbody.velocity.magnitude / decelerationTime * 2;
            Vector2 oppositeForce = -bulletRigidbody.velocity.normalized * decelerationRate;
            bulletRigidbody.AddForce(oppositeForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCasted = false;
        //TODO: risolvere questione layer
        if (collision.gameObject.GetComponent<EnemyScript>() != null)
        {
            var enemy = collision.gameObject.GetComponent<EnemyScript>();
            if (magia == null) { return; }
            else if (LayerMaskExtensions.IsInLayerMask(collision.gameObject, damageMask))
            {
                Debug.Log("Preso");
                enemy.TakeDamage(magia.dannoDellaMagia, magia.tipoMagia);
            }
        }
        if (LayerMaskExtensions.IsInLayerMask(collision.gameObject, damageMask))
        {
            if (collision.GetComponent<Rigidbody2D>() != null)
            {
                collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - gameObject.transform.position).normalized * ExplosionKnockbackForce * 10);
            }
        }
        if (!LayerMaskExtensions.IsInLayerMask(collision.gameObject, ignoreContact))
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// set the damage layer that needs to check
    /// </summary>
    /// <param name="damageValue"></param>
 
    private void OnDisable()
    {
        magia = null;
    }
    private void OnDestroy()
    {
        GameObject expl = Instantiate(ExplosionPref, gameObject.transform.position, gameObject.transform.rotation);
        if (expl != null)
        {
            Destroy(expl, 2);
        }
        else
        {
            Debug.LogWarning("Manca il component ExplosionDamage all'explosion prefab");
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
        Destroy(gameObject, MaxTime);
    }

}
