using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalButton : MonoBehaviour
{
    public TipoMagia activationElement;

    public GameObject doorToOpen;

    public bool finalDoor;

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
                    Destroy(doorToOpen);
                }
            }
            else if(bulletElement == activationElement)
            {
                Destroy(doorToOpen);
            }
        }
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
