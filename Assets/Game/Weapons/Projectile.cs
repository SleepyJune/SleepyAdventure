
using UnityEngine;

public abstract class Projectile : Entity
{
    [System.NonSerialized]
    public new Rigidbody rigidbody;
    [System.NonSerialized]
    public new Collider collider;

    public int speed = 1000;
    public int maxDistance = 5;

    [System.NonSerialized]
    public Unit source;

    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;

    public int damage;

    protected bool destroyed = false;

    protected virtual void Initialize()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        
        //var timeFlying = maxDistance/speed;        
    }

    public void SetVelocity()
    {
        if (speed > 0)
        {
            var dir = (end - start).normalized;
            //rigidbody.velocity = dir * speed;
            rigidbody.AddForce(dir * speed);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (destroyed) return;

        var monster = collision.gameObject.GetComponent<Monster>();
        if(monster != null)
        {
            monster.TakeDamage(source, 10);
            destroyed = true;
            Destroy(this.transform.gameObject);
        }
    }
}
