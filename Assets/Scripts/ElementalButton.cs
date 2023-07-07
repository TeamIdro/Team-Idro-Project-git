using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementalButton : MonoBehaviour
{
    public TipoMagia activationElement;

    public GameObject doorToOpen;

    public bool finalDoor;
    
    [SerializeField] private SpriteRenderer doorClosedsp;
    
    private const string DoorChildName = "Door_2";

    private void Awake()
    {
        doorClosedsp = FindClosedDoorChild();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Magia magicBullet = other.gameObject.GetComponent<Magia>();
        //MAGIA.CS
        //ONDISABLE(){MAGIA = NULL}
        if(magicBullet != null)
        {
            MagiaSO magicSO = magicBullet.magia;
            TipoMagia bulletElement = magicSO.tipoMagia;

            if(finalDoor)
            {
                if(!CheckIfThereIsEnemies())
                {
                    OpenDoor();
                }
            }
            else if(bulletElement == activationElement)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        doorClosedsp.enabled = false;
        doorClosedsp = null;
        doorToOpen.GetComponent<BoxCollider2D>().enabled = false;
    }

    private SpriteRenderer FindClosedDoorChild()
    {
        foreach (SpriteRenderer child in doorToOpen.GetComponentsInChildren<SpriteRenderer>())
        {
            Debug.Log("SEARCHING");
            if (child.sprite.name == DoorChildName)
            {
               return child;
            }
        }

        return null;
    }
    
    private bool CheckIfThereIsEnemies()
    {
        EnemyScript[] enemiesInScene = FindObjectsOfType<EnemyScript>();

        if(enemiesInScene.Length > 0)
        {
            return true;
        }

        return false;
    }
}
