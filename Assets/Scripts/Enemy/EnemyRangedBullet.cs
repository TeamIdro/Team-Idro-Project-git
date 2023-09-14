using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedBullet : MonoBehaviour
{
   public float damage = 5f;
   public float timeToDeath = 5f;
   float timer = 0;
    Vector2 lastVelocity = Vector2.negativeInfinity;

    private void Awake()
    {
        //
    }
    void Update()
   {
        if (lastVelocity == Vector2.negativeInfinity)
        {
            timer += Time.deltaTime;
            if (timer >= timeToDeath) 
            {
                Destroy(this.gameObject);
            }
        }
   }

   void OnTriggerEnter2D(Collider2D other)
   {
      if(other.gameObject.GetComponent<PlayerCharacterController>())
      {
         //Damage Player
            other.gameObject.GetComponent<PlayerCharacterController>().GetDamage(damage);
            other.gameObject.GetComponent<PlayerCharacterController>().KnockBack(this.transform);
            Destroy(this.gameObject);
      }

      Destroy(this.gameObject);
   }

    public void PauseObject()
    {
        lastVelocity = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<ParticleSystem>().Pause();

    }

    public void StartObject()
    {
        GetComponent<Rigidbody2D>().velocity = lastVelocity;
        GetComponent<ParticleSystem>().Play();
        lastVelocity = Vector2.negativeInfinity;
    }

}
