using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MelonMovement : AppleMovement
{
    public CircularSpell explosion;
        
    void Update()
    {
        if (isDead)
        {
            return;
        }
                
        Move();
        Attack();

        if (GameManager.time - lastUpdate > updateFrequency)
        {
            GetDestination();
            Idle();
            lastUpdate = GameManager.time;
        }

    }

    public new void GetDestination()
    {
        var player = GameManager.instance.player;
        if (player != null)
        {
            var playerPos = player.transform.position;

            if (!aggro && playerPos.Distance(transform.position) <= senseRange)
            {
                path = Pathfinding.GetPath(this, transform.position, player.pos, Pathfinding.PathType.Straight);

                if(path != null)
                {
                    emojiBar.anim.SetTrigger("Spotted");
                    aggro = true;
                }                
            }
        }
    }    

    public new void Attack()
    {
        if (GameManager.time - lastAttack > attackFrequency)
        {
            var pos = GameManager.instance.player.pos2d;

            if (pos2d.Distance(pos) < 2 || (aggro && nextPos == IPosition.zero))
            {
                anim.SetTrigger("Attack");
                
                GameManager.instance.CreateCircularSpell(this, explosion, this.transform.position);                
                lastAttack = GameManager.time;

                Death();
            }
        }
    }
}
