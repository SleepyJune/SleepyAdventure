﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public float speed = 1.0f;
    public PathInfo path;
    
    public int health;
    
    public Animator anim;
    public Rigidbody rb;

    //public new Renderer renderer;

    [System.NonSerialized]
    public new Collider collider;

    [System.NonSerialized]
    public IPosition nextPos = IPosition.zero;
    [System.NonSerialized]
    public bool canMove = true;

    protected virtual void Initialize()
    {
        //renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
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

    public PathInfo UnitMoveTo(Vector3 to)
    {
        return GameManager.instance.UnitMoveTo(this, to);
    }

    public void DeleteUnit(float time = 0)
    {
        GameManager.instance.DeleteUnit(this, time);
    }

    public float GetPixelSize()
    {
        var distance = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        float pixelSize = (collider.bounds.extents.magnitude * Mathf.Rad2Deg * Screen.height) 
                            / (distance * Camera.main.fieldOfView);
        return pixelSize;
    }
}