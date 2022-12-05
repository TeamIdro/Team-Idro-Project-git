using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxBehaviour",menuName = "Object Behaviour/Box Behaviour")]
public class BoxBehaviour : ObjectBehaviourSOBase
{

    public override void Interact()
    {
        MoveBox();
    }

   

    public override void ShowUIOnCloseDistance(bool showUI)
    {

    }
    
    private void MoveBox()
    {

    }
}
