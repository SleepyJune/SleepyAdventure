using UnityEngine;

public class LinearSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;

    public int speed = 1000;
    public int maxDistance = 5;

    public GameObject particleOnHit;

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
        var dir = (end - start).normalized;
        dir.y = 0;

        if (speed > 0)
        {            
            //rigidbody.velocity = dir * speed;
            rigidbody.AddForce(dir * speed);
        }        

        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rigidbody.MoveRotation(newRotation);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;
                
        var monster = collision.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(source, 10);

            if (particleOnHit)
            {
                Instantiate(particleOnHit, monster.anim.transform);
            }

            isDead = true;
            Destroy(this.transform.gameObject);
        }
    }
}
