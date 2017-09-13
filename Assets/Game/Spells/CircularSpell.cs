using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;

    public float damageDelay = 0.25f;
    public float deathTime = 1f;

    //private HashSet<Unit> affectedList;
        
    void Awake()
    {
        Initialize();

        if(damageDelay > 0)
        {
            collider.enabled = false;
            DelayAction.Add(() => { collider.enabled = true; }, damageDelay);
        }

        DelayAction.Add(Death, deathTime);
    }

    void Update()
    {
        if (isDead)
        {
            collider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {       
        var unit = collision.gameObject.GetComponent<AttackableUnit>();
        if (unit != null)
        {
            unit.TakeDamage(source, damage);
        }        
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}