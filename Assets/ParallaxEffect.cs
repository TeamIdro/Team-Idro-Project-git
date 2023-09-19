using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxScale = 0.5f; // Controllo la velocità del parallasse
    public float smoothing = 1f; // Controllo la fluidità del movimento

    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = PlayerCharacterController.Instance.gameObject.transform.position;
    }

    void Update()
    {

        float parallax = (previousPlayerPosition.x - PlayerCharacterController.Instance.gameObject.transform.position.x) * parallaxScale;

        Vector3 targetPosition = transform.position + Vector3.right * parallax;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        previousPlayerPosition = PlayerCharacterController.Instance.gameObject.transform.position;
    }
}
