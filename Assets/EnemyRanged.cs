using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private bool attackCooldown;
    public float timeCoolDown = 1.2f;
    public GameObject bullet;
    Coroutine attackCoroutine;

    public void AttackEvent()//GameObject player
    {       
        if (!attackCooldown)
        {
            Debug.Log("ATTACK");
            GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
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
