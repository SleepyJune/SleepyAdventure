
using UnityEngine;

public abstract class Monster : AttackableUnit
{
    public float updateFrequency = 0.25f;
    protected float lastUpdate = 0;

    public float senseRange = 10;
    
    public Transform emojiBarTransform;

    [System.NonSerialized]
    public EmojiBar emojiBar;

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
        anim.SetTrigger("Die");
        anim.SetBool("isDead", true);

        this.GetComponentInChildren<Renderer>().material.SetTransparentMode();

        this.DeleteUnit(1);
    }
}
