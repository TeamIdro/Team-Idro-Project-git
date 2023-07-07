using UnityEngine;

public class AttackBodyDeactivate : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerCharacterController>())
        {
            other.gameObject.GetComponent<PlayerCharacterController>().GetDamage(damage);
            
            this.gameObject.SetActive(false);
        }
    }
}
