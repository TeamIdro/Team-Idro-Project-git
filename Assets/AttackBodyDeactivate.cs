using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBodyDeactivate : MonoBehaviour
{
    public float timeToDeactivate = 1f;
    Coroutine coroutine;

    void OnEnable()
    {
        Debug.Log("INIT DEACTIVATE");
        coroutine = StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(timeToDeactivate);
        Debug.Log("DEACTIVATED");
        this.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Debug.Log("DISABLE");
        StopCoroutine(coroutine);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerCharacterController>())
        {
            Debug.Log("HIT");
        }
    }
}
