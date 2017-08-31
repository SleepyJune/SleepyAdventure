using UnityEngine;

public abstract class Unit : Entity
{
    public float speed = 1.0f;
    public PathInfo path;
    
    public int health;
    
    public Animator anim;
    public Rigidbody rb;

    protected virtual void Initialize()
    {

    }
}
