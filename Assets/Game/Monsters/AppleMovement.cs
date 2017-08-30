using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class AppleMovement : Monster, MonsterMovement
{
    Animator anim;
    Rigidbody rb;

    Vector3 destination;

    float lastUpdate = 0;

    void Awake()
    {
        anim = this.GetComponentInChildren<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody>();

        attackFrequency = 1 / attackSpeed;
    }

    void Update()
    {
        GetDestination();

        if (Time.time - lastUpdate > updateFrequency)
        {
            Move();
            Idle();
            Attack();
            lastUpdate = Time.time;
        }
        
    }

    void GetDestination()
    {
        if (path != null && path.points.Count > 0)
        {            
            var next = path.points.First();
            
            if (next != null)
            {
                if (transform.position.ConvertToIPosition().To2D() == next)
                {
                    path.points.Remove(next);
                }
                else
                {
                    destination = next.ToVector();
                }
            }

        }
        else
        {
            path = null;
        }

        if (destination != Vector3.zero)
        {
            double distance = transform.position.ConvertToIPosition().To2D()
                                .Distance(destination.ConvertToIPosition().To2D());
            
            if (distance > 0.05)
            {
                Vector3 dir = (destination - transform.position).normalized;
                dir.y = 0;

                if (distance >= .1)
                {
                    transform.position += dir * speed * Time.deltaTime;

                    anim.SetFloat("Speed", speed * Time.deltaTime);
                }
                else
                {
                    transform.position = new Vector3(0, transform.position.y, 0)
                            + transform.position.ConvertToIPosition().To2D().ToVector();
                }

                Quaternion newRotation = Quaternion.LookRotation(dir);
                rb.MoveRotation(newRotation);                
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }
        }
    }

    public void Move()
    {        
        if(GameManager.instance.player != null)
        {
            path = this.UnitMoveTo(GameManager.instance.player.transform.position);
            if(path != null)
            {

            }

        }
    }

    public void Idle()
    {
        var isMoving = anim.GetBool("isMoving");

        if (!isMoving && GameManager.instance.player != null)
        {
            var lookat = GameManager.instance.player.transform.position;

            Vector3 dir = (lookat - transform.position).normalized;
            dir.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.MoveRotation(newRotation);

        }
    }

    public void Attack()
    {
        if (Time.time - lastAttack > attackFrequency)
        {

            var pos = GameManager.instance.player.transform.position.ConvertToIPosition();

            if (transform.position.ConvertToIPosition().Distance(pos) < 2)
            {
                GameManager.instance.player.GetComponent<PlayerHealth>().TakeDamage(this, 5);
                lastAttack = Time.time;
            }
        }
    }
}
