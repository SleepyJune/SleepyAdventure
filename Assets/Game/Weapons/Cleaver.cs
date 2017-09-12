using UnityEngine;

public class Cleaver : Weapon
{    
    public LinearSpell proj;

    public override bool Attack(AttackableUnit source, Vector3 start, Vector3 end)
    {
        if (GameManager.time - source.GetLastAttackTime() > source.attackFrequency)
        {
            //var pos = source.transform.position + source.transform.forward * 10;
            GameManager.instance.CreateLinearSpell(source, proj, source.transform.position, end);

            source.anim.SetTrigger("Punch");
            source.SetLastAttackTime(GameManager.time);
            source.LookAt(end);

            return true;
        }

        return false;
    }
}