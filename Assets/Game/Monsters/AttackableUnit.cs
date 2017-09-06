
public abstract class AttackableUnit : Unit
{
    protected float attackSpeed = 1;

    protected float lastAttack = 0;
    public float attackFrequency = 1;

    //public float attackWindupRatio = 0.25f;
    
    protected override void Initialize()
    {
        base.Initialize();
        //attackFrequency = 1 / attackSpeed;
        attackSpeed = 1 / attackFrequency;
    }

    public abstract void TakeDamage(Unit source, int amount);

    public float GetLastAttackTime()
    {
        return lastAttack;
    }  
    
    public void SetLastAttackTime(float time)
    {
        lastAttack = time;
    }  
}
