using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedBullet : MonoBehaviour
{
   public int damage = 5;
   public float timeToDeath = 5f;
   float timer = 0;

   void Update()
   {
      timer += Time.deltaTime;
      if(timer >= timeToDeath)
      {
         Destroy(this.gameObject);
      }
   }

   void OnTriggerEnter2D(Collider2D other)
   {
      if(other.gameObject.GetComponent<PlayerCharacterController>())
      {
         //Damage Player
         other.gameObject.GetComponent<PlayerCharacterController>().GetDamage(damage);
         Destroy(this.gameObject);
      }
   }
}
