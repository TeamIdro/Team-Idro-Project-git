using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;

    public static SpawnManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject actualSpawnPoint;

    void Awake()
    {
        _instance = this;
    }

    public void SetSpawnPoint(GameObject sp)
    {
        actualSpawnPoint = sp;
    }

    public void Respawn()
    {
        PlayerCharacterController.Instance.transform.position 
            = actualSpawnPoint.transform.position + new Vector3(0f, 1.5f);
    }
}
