using System.Linq;
using UnityEngine;

public class Punch : Weapon
{

    public override bool Attack(AttackableUnit source)
    {
        if (GameManager.time - source.GetLastAttackTime() > source.attackFrequency)
        {
            var enemies = GameManager.instance.units.Values.Where(u => u is Monster);

            foreach (Monster enemy in enemies)
            {
                if (enemy.transform.position.ConvertToIPosition().To2D()
                    .Distance(source.transform.position.ConvertToIPosition().To2D()) < 2)
                {
                    var dir = enemy.transform.position - source.transform.position;
                    dir.y = 0;

                    enemy.transform.GetComponent<Rigidbody>().AddForce(1000 * dir);

                    enemy.TakeDamage(source, 100);

                }
            }

            source.anim.SetTrigger("Punch");
            source.SetLastAttackTime(GameManager.time);

            return true;
        }

        return false;
    }
}