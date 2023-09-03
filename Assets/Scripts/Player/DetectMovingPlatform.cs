using UnityEngine;



public class DetectMovingPlatform : MonoBehaviour
{
    public Vector2 boxCastDimension;
    public float maxCastDistance;
    public LayerMask terrainMask;
    RaycastHit2D hit;
    public float offsetXCast;
    public float jumpForceBoost;
    private float defaultJumpForce;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        defaultJumpForce = GetComponentInParent<PlayerCharacterController>().jumpVelocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("MovingPlatform"))   
        {
            Debug.Log("entrato");
            this.transform.parent.transform.SetParent(other.transform);
                            
            float platformVelocity = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.x;
            if(platformVelocity > 0f && rb.velocity.x > 0f
                || platformVelocity < 0f && rb.velocity.x < 0f)
            {
                GetComponentInParent<PlayerCharacterController>().jumpVelocity = defaultJumpForce;
            }
            else if(platformVelocity == 0f)
            {
                GetComponentInParent<PlayerCharacterController>().jumpVelocity = defaultJumpForce;
            }
            else
            {
                GetComponentInParent<PlayerCharacterController>().jumpVelocity = jumpForceBoost;
            }

        }
        // else
        // {
        //     this.transform.parent?.transform.SetParent(null);
        //     GetComponentInParent<PlayerCharacterController>().jumpVelocity = 700f;
        // }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("MovingPlatform"))
            // && this.transform.parent?.transform == other.transform)
        {
            Debug.Log("uscito");
            this.transform.parent?.transform.SetParent(null);
            GetComponentInParent<PlayerCharacterController>().jumpVelocity = 700f;
        }
    }

    
}
