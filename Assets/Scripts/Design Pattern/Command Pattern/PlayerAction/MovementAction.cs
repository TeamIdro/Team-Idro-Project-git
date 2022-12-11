using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : ActionBase
{
    public Vector2 direction;
    public Vector2 previousDirection;
    private KidControllerOnChessBoard controller;
    public MovementAction(KidControllerOnChessBoard controller,Vector2 direction) : base(controller)
    {
        this.controller = controller;
        this.direction = direction;
    }

    public override void Execute()
    {
        previousDirection = controller.gameObject.transform.position;
        controller.gameObject.transform.position = direction;
        if(controller.CurrentTimeAvailable>0)controller.CurrentTimeAvailable--;
    }

    public override void Undo()
    {
        controller.gameObject.transform.position = previousDirection;
        controller.CurrentTimeAvailable++;
    }
}
