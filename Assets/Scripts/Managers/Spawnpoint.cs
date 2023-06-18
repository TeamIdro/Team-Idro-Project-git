using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerCharacterController>())
        {
            SpawnManager.Instance.SetSpawnPoint(this.gameObject);
        }
    }
}
