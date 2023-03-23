
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IEnemy
{
     #region Methods

     // public EnemyCategory category {get; set;}

     // public Weakness weakness {get; set;}

     public int hp {get; set;}

     public int attack {get; set;}

     public float speed {get; set;}

     public Rigidbody2D rigidBody {get; set;}

     public SpriteRenderer spriteRenderer {get; set;}
     
     #endregion
}
