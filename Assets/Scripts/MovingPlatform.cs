using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool moving = false;

    void Awake()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if(this.transform.rotation.eulerAngles.x != 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
}
