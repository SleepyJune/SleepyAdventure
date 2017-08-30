using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class FenceDoor : MonoBehaviour, Interactable
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
        if(source.tag != "Player")
        {
            return false;
        }

        if (Time.time - lastUseTime > reUseableTime)
        {
            var isDoorOpen = anim.GetBool("isOpen");

            anim.SetBool("isOpen", !isDoorOpen);
            boxCollider.isTrigger = !isDoorOpen;

            lastUseTime = Time.time;
            return true;
        }

        return false;
    }
}
