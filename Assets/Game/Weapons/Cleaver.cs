using UnityEngine;

public class Cleaver : Weapon
{
    Projectile proj;

    void Awake()
    {
        proj = GetComponent<LinearProjectile>();
    }

    public override bool Attack(AttackUnit source)
    {
        if (Time.time - source.GetLastAttackTime() > source.attackFrequency)
        {
            var pos = source.transform.position + source.transform.forward * 10;
            GameManager.instance.CreateProjectile(source, proj, source.transform.position, pos);

            source.anim.SetTrigger("Punch");
            source.SetLastAttackTime(Time.time);

            return true;
        }

        return false;
    }
}