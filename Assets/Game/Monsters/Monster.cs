
using UnityEngine;

public abstract class Monster : AttackUnit
{
    public float updateFrequency = 0.25f;

    [System.NonSerialized]
    public bool isDead = false;

    /*protected override void Initialize()
    {
        base.Initialize();
    }*/

    public void TakeDamage(Unit source, int amount)
    {
        health -= amount;

        GameManager.instance.CreateDamageText(this, -amount);
    }

    public void Death()
    {
        anim.SetTrigger("Die");
        anim.SetBool("isDead", true);

        this.GetComponentInChildren<Renderer>().material.SetTransparentMode();

        this.DeleteUnit(1);
    }
}
