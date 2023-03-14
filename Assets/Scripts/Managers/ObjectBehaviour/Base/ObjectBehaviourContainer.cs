using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviourContainer : MonoBehaviour
{
    public ObjectBehaviourSOBase behaviourScriptableObject;
    public bool useonlyOnce;

    private void OnMouseEnter()
    {
        behaviourScriptableObject.ChangeCursor();
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(GameManager.Instance.baseMouseCursor,Vector2.zero,CursorMode.ForceSoftware);
    }
}
