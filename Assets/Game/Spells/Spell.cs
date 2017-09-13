using System.Collections;
using UnityEngine;

public abstract class Spell : Entity
{
    [System.NonSerialized]
    public new Rigidbody rigidbody;
    [System.NonSerialized]
    public new Collider collider;

    [System.NonSerialized]
    public Unit source;

    public int damage;
        
    protected virtual void Initialize()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        
        if(collider == null)
        {
            collider = GetComponentInChildren<Collider>();
        }

        //var timeFlying = maxDistance/speed;        
    }

    public abstract void Death(); //spell destruction and clean up
}
