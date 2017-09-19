using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

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

        /*if(damageDelay > 0)
        {
            collider.enabled = false;
            DelayAction.Add(() => { collider.enabled = true; }, damageDelay);
        }*/

        if (damageDelay > 0)
        {
            DelayAction.Add(() => OnSpellActivate(), damageDelay);
        }
        else
        {
            OnSpellActivate();
        }
                
        DelayAction.Add(Death, deathTime);
    }

    void OnSpellActivate()
    {
        var pos2d = transform.position.ConvertToIPosition2D();

        var affectedUnits = GameManager.instance.units.Values
            .Where(u => !u.isDead && u is AttackableUnit && u.pos2d.Distance(pos2d) <= radius).ToList();

        var player = GameManager.instance.player;

        if (player != null && !player.isDead)
        {
            if(player.pos2d.Distance(pos2d) <= radius)
            {
                affectedUnits.Add(GameManager.instance.player);
            }
        }

        foreach(AttackableUnit unit in affectedUnits)
        {
            unit.TakeDamage(source, damage);
        }        
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}