using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RigidbodyObject : RMonoBehaviour
{
    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rigidbody)
                _rigidbody = GetComponentInChildren<Rigidbody>();
            return _rigidbody;
        }
    }
    public Transform Transform
    {
        get
        {
            if (!_transform)
                _transform = transform;
            return _transform;
        }
    }
    private Transform _transform;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _transform = transform;
    }
}
