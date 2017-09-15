using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AppleMovement : Monster, MonsterMovement
{
    //IPosition nextPos = IPosition.zero;
        
    void Awake()
    {
        if (GameManager.instance) //check if it's editor or game
        {
            Initialize();
        }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        Move();

        if (GameManager.time - lastUpdate > updateFrequency)
        {
            GetDestination();
            Idle();
            Attack();
            lastUpdate = GameManager.time;
        }
        
    }

    protected void Move()
    {
        if (path != null && path.points.Count > 0)
        {
            var next = path.points.First();
            
            if (next != null)
            {
                if (pos2d == next)
                {
                    path.points.Remove(next);

                    if (path.points.Count > 0)
                    {
                        next = path.points.First();
                        if (next != null)
                        {
                            nextPos = next;
                        }
                    }
                }
                else
                {
                    nextPos = next;
                }
            }

        }
        else
        {
            path = null;
        }
        
        if(nextPos != IPosition.zero &&
            GameManager.instance.SameDestination(this, nextPos))
        {
            nextPos = IPosition.zero;
            path = null;
        }
        
        if(nextPos != IPosition.zero &&
            Pathfinding.CanWalkToSquare(this, nextPos) == false)
        {            
            nextPos = IPosition.zero;
            path = null;
        }

        if (nextPos != IPosition.zero)
        {
            double distance = Vector3.Distance(transform.position.To2D(), nextPos.ToVector());

            if (distance > 0.05)
            {
                Vector3 dir = (nextPos.ToVector() - transform.position).normalized;
                dir.y = 0;

                if (distance >= .1)
                {
                    transform.position += dir * speed * Time.deltaTime;

                    anim.SetFloat("Speed", speed);
                }
                else
                {
                    transform.position = new Vector3(0, transform.position.y, 0)
                            + pos2d.ToVector();
                }

                Quaternion newRotation = Quaternion.LookRotation(dir);
                rigidbody.MoveRotation(newRotation);                
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }

        }
    }

    public void GetDestination()
    {
        var player = GameManager.instance.player;
        if (player != null)
        {
            var playerPos = player.transform.position;

            if(playerPos.Distance(transform.position) <= senseRange)
            {
                path = this.UnitMoveTo(GameManager.instance.player.transform.position);
                emojiBar.anim.SetTrigger("Spotted");
            }
            else
            {
                emojiBar.anim.SetTrigger("Idle");
            }

        }
    }

    public void Idle()
    {
        var isMoving = anim.GetBool("isMoving");

        if (!isMoving && GameManager.instance.player != null)
        {
            var playerPos = GameManager.instance.player.transform.position;

            if (playerPos.Distance(transform.position) <= senseRange)
            {                
                Vector3 dir = (playerPos - transform.position).normalized;
                dir.y = 0;

                Quaternion newRotation = Quaternion.LookRotation(dir);
                rigidbody.MoveRotation(newRotation);
            }

        }
    }

    public void Attack()
    {
        if (GameManager.time - lastAttack > attackFrequency)
        {

            var pos = GameManager.instance.player.pos2d;

            if (pos2d.Distance(pos) < 2)
            {
                anim.SetTrigger("Attack");

                //GameManager.instance.player.GetComponent<PlayerHealth>().TakeDamage(this, 5);
                GameManager.instance.player.GetComponent<PlayerMovement>().TakeDamage(this, 5);

                lastAttack = GameManager.time;
            }
        }
    }
}
