﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MelonMovement : AppleMovement
{
    public Explosion explosion;

    void Update()
    {
        Move();

        if (Time.time - lastUpdate > updateFrequency)
        {
            GetDestination();
            Idle();
            Attack();
            lastUpdate = Time.time;
        }

    }

    public new void Attack()
    {
        if (Time.time - lastAttack > attackFrequency)
        {
            var pos = GameManager.instance.player.pos2d;

            if (pos2d.Distance(pos) < 2)
            {
                anim.SetTrigger("Attack");


                //GameManager.instance.player.GetComponent<PlayerHealth>().TakeDamage(this, 5);
                GameManager.instance.CreateProjectile(this, explosion, this.transform.position, this.transform.position);


                lastAttack = Time.time;
            }
        }
    }
}
