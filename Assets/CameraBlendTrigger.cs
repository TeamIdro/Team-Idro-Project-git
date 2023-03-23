using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlendTrigger : MonoBehaviour
{
   [SerializeField] CinemachineVirtualCamera cameraDellaCamera;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            cameraDellaCamera.Priority = 11;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacterController>() != null)
        {
            cameraDellaCamera.Priority = 0;
        }
    }
}
