using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectMovingPlatform : MonoBehaviour
{
    public Vector2 boxCastDimension;
    public float maxCastDistance;
    public LayerMask terrainMask;
    RaycastHit2D hit;

    void Awake()
    {
        // terrainMask = LayerMask.GetMask("Terreno");    
    }

    private void Update() 
    {
        hit = Physics2D.BoxCast(transform.position, boxCastDimension, 0, Vector2.down, maxCastDistance, terrainMask); 

        if(hit.collider != null)
        {
            if(hit.collider.CompareTag("MovingPlatform"))
            {
                this.transform.SetParent(hit.transform);
            }
            else
            {
                this.transform.SetParent(null);
            }
        }
        else
        {
            this.transform.SetParent(null);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(new Vector3(transform.position.x,transform.position.y - maxCastDistance, 0), boxCastDimension);
    // }
}
