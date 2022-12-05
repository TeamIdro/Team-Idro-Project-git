using UnityEngine;

public abstract class ObjectBehaviourSOBase : ScriptableObject
{
    public abstract void Interact();
    public abstract void ShowUIOnCloseDistance(bool showUI);
}

