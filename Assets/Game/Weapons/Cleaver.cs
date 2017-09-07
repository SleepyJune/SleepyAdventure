using UnityEngine;

public class Cleaver : Weapon
{
    public LinearSpell proj;

    public override bool Attack(AttackableUnit source)
    {
        if (GameManager.time - source.GetLastAttackTime() > source.attackFrequency)
        {
            var pos = source.transform.position + source.transform.forward * 10;
            GameManager.instance.CreateLinearSpell(source, proj, source.transform.position, pos);

            source.anim.SetTrigger("Punch");
            source.SetLastAttackTime(GameManager.time);

            return true;
        }

        return false;
    }
}