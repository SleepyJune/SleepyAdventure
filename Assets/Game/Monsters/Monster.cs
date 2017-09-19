
using UnityEngine;

public abstract class Monster : AttackableUnit
{
    public float updateFrequency = 0.25f;
    protected float lastUpdate = 0;

    public float senseRange = 10;
    
    public Transform emojiBarTransform;

    [System.NonSerialized]
    public EmojiBar emojiBar;

    public AnimationClip deathAnimation;

    [System.NonSerialized]
    public bool aggro = false;

    protected override void Initialize()
    {
        base.Initialize();
        emojiBar = GameManager.instance.CreateEmojiBar(this);
    }

    public override void TakeDamage(Unit source, int amount)
    {
        health -= amount;
        GameManager.instance.CreateDamageText(this, -amount);
    }

    public void Death()
    {
        isDead = true;

        anim.SetTrigger("Die");
        anim.SetBool("isDead", true);

        //this.GetComponentInChildren<Renderer>().material.SetTransparentMode();


        if (deathAnimation)
        {
            DeleteUnit(deathAnimation.length);
        }
        else
        {
            DeleteUnit(0);
        }

    }
}
