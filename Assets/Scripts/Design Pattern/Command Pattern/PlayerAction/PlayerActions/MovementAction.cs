using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementAction : ActionBase
{
    public Vector2 direction;
    public Vector2 previousDirection;
    private KidControllerOnChessBoard controller;
    public MovementAction(KidControllerOnChessBoard controller,Vector2 direction,Vector2 previousDirection)
    {
        this.controller = controller;
        this.direction = direction;
        this.previousDirection = previousDirection;
    }

    public void Execute()
    {
        previousDirection = controller.gameObject.transform.position;
        controller.gameObject.transform.position = direction;
        if (controller.CurrentTimeAvailable > 0) controller.CurrentTimeAvailable--;
    }

    public void Undo()
    {
        controller.gameObject.transform.position = previousDirection;
        controller.CurrentTimeAvailable++;
    }
}
