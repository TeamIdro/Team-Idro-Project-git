using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyFly : MonoBehaviour
{

    public bool attackCooldown = false;
    public float timeCoolDown = 1.2f;

    public GameObject attackBody;

    Coroutine attackCoroutine;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void AttackEvent()
    {
        if (!attackCooldown)
        {
            attackCooldown = true;
            _animator.SetTrigger("Attack");
        }
    }
    
    private void SetIdleAnimation()
    {
        _animator.SetTrigger("Idle");
    }

    private void ActivateAttackBody()
    {
        attackBody.SetActive(true);
    }
    
    private void DeactivateAttackBody()
    {
        attackBody.SetActive(false);
    }
}