using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRecorder : MonoBehaviour
{
    private readonly Stack<ActionBase> actionBaseStack = new Stack<ActionBase>();
    public void Record(ActionBase action)
    {
        actionBaseStack.Push(action);
        action.Execute();
       
    }
    public void Rewind()
    {
        if (actionBaseStack.Count > 0)
        {
            var action = actionBaseStack.Pop();
            action.Undo();
        }
       
    }
}
