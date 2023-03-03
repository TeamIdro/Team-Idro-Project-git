using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public void destroyAfterTime(float MaxTime)
    {
        Destroy(gameObject, MaxTime);
    }
}
