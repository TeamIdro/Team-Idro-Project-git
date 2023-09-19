using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxScale = 0.5f; // Controllo la velocit� del parallasse
    public float smoothing = 1f; // Controllo la fluidit� del movimento

    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = PlayerCharacterController.Instance.gameObject.transform.position;
    }

    void Update()
    {

        float parallaxX = (previousPlayerPosition.x - PlayerCharacterController.Instance.gameObject.transform.position.x) * parallaxScale;
        float parallaxY = (previousPlayerPosition.y - PlayerCharacterController.Instance.gameObject.transform.position.y) * parallaxScale;

        Vector3 targetPosition = transform.position + new Vector3(parallaxX, parallaxY, 0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        previousPlayerPosition = PlayerCharacterController.Instance.gameObject.transform.position;
    }
}
