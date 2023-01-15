using UnityEngine;

public interface ITrigger
{
    string myTag { get; set; }
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}

public interface ICollision
{
    string myTag { get; set; }
    void OnCollisionEnter(Collision other);
    void OnCollisionExit(Collision other);
}
