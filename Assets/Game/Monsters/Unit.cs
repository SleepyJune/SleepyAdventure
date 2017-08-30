using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float speed = 1.0f;
    public PathInfo path;
    
    public int health;

    public int id;

    public Animator anim;
    public Rigidbody rb;

    protected virtual void Initialize()
    {

    }
}
