using UnityEngine;
using UnityEngine.InputSystem;
public class Interacter : MonoBehaviour
{
    GamePlayInputActions m_inputActions;
    ObjectBehaviourContainer objectBehaviour;
    private bool m_canInteract = false;
    private void Awake()
    {
        m_inputActions = new GamePlayInputActions();
    }
    private void OnEnable()
    {
        m_inputActions.Enable();
    }
    private void OnDisable()
    {
        m_inputActions.Disable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ObjectBehaviourContainer>() != null)
        {
            objectBehaviour = collision.GetComponent<ObjectBehaviourContainer>();
            objectBehaviour.behaviourScriptableObject.ShowUIOnCloseDistance(true);
            m_canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ObjectBehaviourContainer>() != null)
        {
            objectBehaviour = collision.GetComponent<ObjectBehaviourContainer>();
            objectBehaviour.behaviourScriptableObject.ShowUIOnCloseDistance(false);
            objectBehaviour = null;
            m_canInteract=false;
        }
    }
    private void Update()
    {
        InteractionUpdate();
    }

    private void InteractionUpdate()
    {
        if (m_canInteract)
        {
            if (objectBehaviour != null)
            {
                if (m_inputActions.GamePlay.Interaction.WasPressedThisFrame())
                {
                    objectBehaviour.behaviourScriptableObject.Interact();
                    if (objectBehaviour.useonlyOnce)
                    {
                        Destroy(objectBehaviour.gameObject);
                    }
                }


            }
        }
    }
}
