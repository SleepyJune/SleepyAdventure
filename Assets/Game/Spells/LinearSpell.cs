using UnityEngine;

public class LinearSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;

    public int speed = 1000;
    public int maxDistance = 5;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (Vector3.Distance(start, transform.position) > maxDistance)
        {
            Destroy(transform.gameObject);
        }
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
        if (isDead) return;

        var monster = collision.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(source, 10);
            isDead = true;
            Destroy(this.transform.gameObject);
        }
    }
}
