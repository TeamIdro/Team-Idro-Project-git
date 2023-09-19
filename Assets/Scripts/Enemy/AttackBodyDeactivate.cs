using UnityEngine;

public class AttackBodyDeactivate : MonoBehaviour
{
    public float damage = 10f;
    private bool isFirstAttack = true;

    private void OnEnable()
    {
        isFirstAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerCharacterController>()
            && isFirstAttack)
        {

            other.gameObject.GetComponent<PlayerCharacterController>().GetDamage(damage);
            isFirstAttack = false;
            this.gameObject.SetActive(false);
        }
    }


}
