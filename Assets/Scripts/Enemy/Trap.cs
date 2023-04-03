using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerCharacterController>();
        if(player != null)
        {
            player.GetDamage(player.hp+1);
        }
    }
}
