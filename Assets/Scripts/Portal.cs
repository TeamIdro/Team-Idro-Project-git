using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
public string sceneName;

void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerCharacterController>();
        if(player != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
