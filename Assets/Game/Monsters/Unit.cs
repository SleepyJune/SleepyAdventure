using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public float speed = 1.0f;
    public PathInfo path;
    
    public int health;
    
    public Animator anim;
    public Rigidbody rb;

    [System.NonSerialized]
    public IPosition nextPos = IPosition.zero;
    public bool canMove = true;

    protected virtual void Initialize()
    {

    }

    public IEnumerator DisableUnitMovementHelper(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void DisableUnitMovement(float time)
    {
        StartCoroutine(DisableUnitMovementHelper(time));
    }
}
