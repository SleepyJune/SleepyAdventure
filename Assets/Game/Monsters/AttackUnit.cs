
public abstract class AttackUnit : Unit
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
}
