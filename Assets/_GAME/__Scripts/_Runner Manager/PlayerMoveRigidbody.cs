using PathCreation;
using Rentire.Core;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveRigidbody : RigidbodyObject, IPlayerMove
{
    public float MaxXUnit = 2f;
    float my_xUnit = 0f;
    float distance = 0;
    Vector3 offset = Vector3.zero;
    public void Move(VertexPath path = null, float speed = 5, float xUnit = 0f)
    {
        if (path != null)
        {
            //5. distance ile yap
            var direction = path.GetDirectionAtDistance(distance);
            var yAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, yAngle, 0);
            Transform.rotation = targetRotation;

            my_xUnit += xUnit;
            my_xUnit = Mathf.Clamp(my_xUnit, -MaxXUnit, MaxXUnit);

            distance += Time.fixedDeltaTime * speed;

            var newPosition = path.GetPointAtDistance(distance);
            Rigidbody.MovePosition(newPosition + Transform.right * my_xUnit);
        }
        else
        {
            var positionForward = Transform.position + Vector3.forward * speed * Time.deltaTime + Transform.right * xUnit;
            positionForward = positionForward.WithX(Mathf.Clamp(positionForward.x, -MaxXUnit, MaxXUnit));
            Rigidbody.MovePosition(positionForward);

        }
    }
}
