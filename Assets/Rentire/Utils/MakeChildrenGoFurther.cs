using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildrenGoFurther : MonoBehaviour
{
    public float UnitToMoveFurtherOnZ = 1f;
    public void MoveFurther()
    {
        float currentZ = 0;
        for (int i = 0; i < transform.childCount; i++)
        {

            var child = transform.GetChild(i);
            child.position = Vector3.forward * currentZ;
            currentZ += UnitToMoveFurtherOnZ;

        }
    }
}
