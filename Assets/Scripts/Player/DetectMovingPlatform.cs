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
        rb = GetComponent<Rigidbody2D>();
        defaultJumpForce = GetComponent<PlayerCharacterController>().jumpVelocity;
    }


    private void Update() 
    {
        //TODO modificare con trigger collider
        hit = Physics2D.BoxCast(transform.position + new Vector3(offsetXCast, 0, 0), boxCastDimension, 0, Vector2.down, maxCastDistance, terrainMask); 

        if(hit.collider != null)
        {

            if(hit.collider.CompareTag("MovingPlatform"))
            {
                this.transform.SetParent(hit.transform);
                
                
                float platformVelocity = hit.collider.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.x;
                if(platformVelocity > 0f && rb.velocity.x > 0f
                    || platformVelocity < 0f && rb.velocity.x < 0f)
                {
                    GetComponent<PlayerCharacterController>().jumpVelocity = defaultJumpForce;
                }
                else if(platformVelocity == 0f)
                {
                    GetComponent<PlayerCharacterController>().jumpVelocity = defaultJumpForce;
                }
                else
                {
                    GetComponent<PlayerCharacterController>().jumpVelocity = jumpForceBoost;
                }

            }
            else
            {
                this.transform.SetParent(null);
                GetComponent<PlayerCharacterController>().jumpVelocity = 700f;
            }
        }
        else
        {
            this.transform.SetParent(null);
            GetComponent<PlayerCharacterController>().jumpVelocity = 700f;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x + offsetXCast, transform.position.y - maxCastDistance, 0), boxCastDimension);
    }
}
