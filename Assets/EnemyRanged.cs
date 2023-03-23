using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private bool attackCooldown;
    public float timeCoolDown = 1.2f;
    public GameObject bullet;
    Coroutine attackCoroutine;

    public float force = 200f;

    public void AttackEvent()//GameObject player
    {       
        if (!attackCooldown)
        {
            Debug.Log("ATTACK");

            Shoot();

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

    public void Shoot()
    {
        var bulletInst = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInst.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(PlayerCharacterController.Instance.transform.position.x - transform.position.x, 
                        PlayerCharacterController.Instance.transform.position.y - transform.position.y) * force, 
                        ForceMode2D.Force);
    }
}
