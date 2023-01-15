using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveCharacter : TransformObject, IPlayerMove
{
    CharacterController characterController;
    
    protected override void Awake()
    {
        base.Awake();
        GetComponentAndAssign(ref characterController);
    }
    public void Move(VertexPath path = null, float speed = 5, float xUnit = 0f)
    {
        if(path != null)
        {
            var closestPoint = path.GetClosestDistanceAlongPath(Transform.position);
            var direction = path.GetDirectionAtDistance(closestPoint);

            characterController.SimpleMove(speed * direction);
            
        }
        else
        {
            characterController.SimpleMove(Vector3.forward * speed);
        }
    }

}
