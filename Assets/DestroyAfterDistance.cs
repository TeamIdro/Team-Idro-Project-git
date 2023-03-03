using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDistance : MonoBehaviour
{
    Vector3 position;
    public float MaxDistance;
    // Start is called before the first frame update
    private void Awake()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(position, transform.position);
        if (distance >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }
}
