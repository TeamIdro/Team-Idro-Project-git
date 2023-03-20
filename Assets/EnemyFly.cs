using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;


public class EnemyFly : MonoBehaviour
{

    public bool attackCooldown;
    public float timeCoolDown = 1.2f;

    // public float attackCooldownSet = 1.2f;

    public GameObject AttackBody;

    Coroutine attackCoroutine;

    public void AttackEvent()//GameObject player
    {       
        if (!attackCooldown)
        {
            Debug.Log("ATTACK");
            AttackBody.SetActive(true);
            attackCooldown = true;
            attackCoroutine = StartCoroutine(CooldownAttack());
        }
    }

    private IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(timeCoolDown);
        attackCooldown = false;
        StopCoroutine(attackCoroutine);
    }
}
