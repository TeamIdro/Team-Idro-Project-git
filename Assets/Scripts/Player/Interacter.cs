using UnityEngine;
using UnityEngine.InputSystem;
public class Interacter : MonoBehaviour
{
    GamePlayInputActions m_inputActions;
    ObjectBehaviourContainer objectBehaviour;
    Vector2 mousePosition;
    public LayerMask maskToInteract;
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
    public void CheckForInteractable()
    {
        RaycastHit2D hit;
        mousePosition = Camera.main.ScreenToWorldPoint(m_inputActions.Kid.MousePosition.ReadValue<Vector2>());
        hit = Physics2D.Raycast(mousePosition, Vector3.zero, 1f, maskToInteract);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.GetComponent<ObjectBehaviourContainer>() != null)
            {
                objectBehaviour = hit.collider.gameObject.GetComponent<ObjectBehaviourContainer>();
                m_canInteract = true;
            }
            else
            {
                objectBehaviour = null;
                m_canInteract = false;
            }
        }
    }
    
    private void Update()
    {
        InteractionUpdate();
    }
    private void FixedUpdate()
    {
        CheckForInteractable();
    }

    private void InteractionUpdate()
    {
        if (m_canInteract)
        {
            if (objectBehaviour != null)
            {
                if (m_inputActions.Kid.MouseClick.WasPressedThisFrame())
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(mousePosition, Vector2.zero);
    }
}
