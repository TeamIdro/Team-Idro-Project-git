using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum EPotionType
{
    Healing = 1,
    Venom = 2,
    Damage = 4,
    Buff = 8

}

[CreateAssetMenu(fileName ="PotionBehaviour",menuName =("Object Behaviour/Potion Behaviour"))]
public class PotionBehaviour : ObjectBehaviourSOBase
{
    
    public float healthRegaind;
    public float damageTaken;
    public EPotionType potionType;
    public override void Interact()
    {
        Debug.Log("interacted");
    }

    public override void ShowUIOnCloseDistance(bool showUI)
    {
        Debug.Log("shown ui");
    }
}
