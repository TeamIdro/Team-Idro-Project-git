//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RaycastWeapon : Weapon
//{
//    [SerializeField]
//    private LineRenderer lr;
//    private Projectile currentProjectile;
//    public float rayLength;
//    private Vector2 rayEnd;
//    public LayerMask layerMask;
//    public float trailTime = 1f;
//    public float lineActiveTime = .2f;
//    public int linePoints = 1;
//    public Transform targetObject;
//    public float lightningNoiseX = .4f;
//    public float lightningNoiseY = .4f;
//    public ParticleSystem lightningExplosion;

//    private void Start()
//    {
//        linePoints = lr.positionCount - 1;
//    }


//    private void Update()
//    {
//        if (isFiring)
//        {
//            if (Time.time - lastFired > 1 / fireRate)
//            {
//                //Shoot();
//                ShootBullet();
//                isFiring = false;
//                lastFired = Time.time;
//            }
//        }
//        //lr.material.SetTextureOffset("_MainTex", Vector2.right * Time.time);
//    }

//    public override void Shoot()
//    {
//        isFiring = true;
//    }
//    public override void StopShoot()
//    {
//        isFiring = false;
//    }


//    private void ShootBullet()
//    {
//        Vector2 firePointPos = firePoint.position;
//        Vector2 endPos = firePointPos + (Vector2)firePoint.right * rayLength;

//        RaycastHit2D hit = Physics2D.Linecast(firePointPos, endPos, layerMask);

//        if (hit)
//        {
//            Debug.Log(hit.transform.name);
//            rayEnd = hit.point;
//        }
//        else
//        {
//            Debug.Log("nah man...................");
//            rayEnd = endPos;
//        }
        
//        lr.enabled = true;

//        var distance = hit.distance;

//        if (distance == 0)
//        {
//            distance = rayLength;
//        }

//        for (int i = 1; i < linePoints; i++)
//        {
//            var pos = lr.GetPosition(i);
            
//            //Debug.Log(rayEnd.x);
//            pos.x = (distance / linePoints * i) + Random.Range(-lightningNoiseX, lightningNoiseX);
//            pos.y += Random.Range(-lightningNoiseY, lightningNoiseY);

//            lr.SetPosition(i, pos);
//        }

//        if (distance < rayLength)
//        {
//            lr.SetPosition(linePoints, new Vector2(distance, 0f));
//            lightningExplosion.transform.position = hit.point;
//        }
//        else
//        {
//            lr.SetPosition(linePoints, new Vector2(rayLength, 0f));
//            lightningExplosion.transform.position = endPos;
            
//        }
//        //Debug.Log(distance.ToString());
//        //Debug.Log(endPos.ToString());
//        lightningExplosion.Play();

//        Invoke("DisableLine", lineActiveTime);
//    }

//    void DisableLine()
//    {
//        lr.enabled = false;
//        for (int i = 1; i < linePoints; i++)
//        {

//            var pos = lr.GetPosition(i);

//            pos.x = i;
//            pos.y = 0f;

//            lr.SetPosition(i, pos);
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.right * rayLength, Color.red);
//    }
//}
