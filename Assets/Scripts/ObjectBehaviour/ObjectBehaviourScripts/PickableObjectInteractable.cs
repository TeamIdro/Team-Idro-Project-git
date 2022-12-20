using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickableObjectInteractable", menuName = "Object Behaviour/PickableObjectInteractable")]
public class PickableObjectInteractable : ObjectBehaviourSOBase
{
    public Texture2D cursorOnMouseEnter;
    public override void ChangeCursor()
    {
        Cursor.SetCursor(cursorOnMouseEnter, Vector2.zero, CursorMode.ForceSoftware);
    }

    public override void Interact()
    {
    }
}
