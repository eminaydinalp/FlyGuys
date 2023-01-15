using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : RMonoBehaviour
{
    public Transform Transform { get {
            if (!_transform)
                _transform = transform;
            return _transform;
        }
    }
    private Transform _transform;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _transform = transform;
    }

}
