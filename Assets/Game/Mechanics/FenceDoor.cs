using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class FenceDoor : Obstacle, Interactable
{
    Animator anim;
    BoxCollider boxCollider;

    float reUseableTime = 1;
    float lastUseTime;

    void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        boxCollider = transform.GetComponent<BoxCollider>();
    }

    public bool Use(Unit source)
    {
        if (!(source is Hero))
        {
            return false;
        }

        if (GameManager.time - lastUseTime > reUseableTime)
        {
            var isDoorOpen = anim.GetBool("isOpen");

            isDoorOpen = !isDoorOpen; //switch the door state

            anim.SetBool("isOpen", isDoorOpen);
            boxCollider.isTrigger = isDoorOpen;

            isWalkable = isDoorOpen;

            if (!isDoorOpen)
            {
                if (!sqr.obstacles.ContainsKey(id))
                {
                    sqr.obstacles.Add(id, this);
                }
            }
            else
            {
                sqr.obstacles.Remove(id);
            }

            lastUseTime = GameManager.time;
            return true;
        }

        return false;
    }
}
