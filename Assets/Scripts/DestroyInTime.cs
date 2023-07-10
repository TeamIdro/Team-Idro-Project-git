using UnityEngine;


public class DestroyInTime : MonoBehaviour
{
    public float timeToDestroy;
    public void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }
}
