using System.Linq;
using UnityEngine;

public class Punch : MeleeWeapon
{
    public override bool Attack(AttackableUnit source, Vector3 start, Vector3 end)
    {
        if (GameManager.time - source.GetLastAttackTime() > source.attackFrequency)
        {
            var enemies = GameManager.instance.units.Values.Where(u => u is Monster);
            var playerTransform = GameManager.instance.player.transform;
                        
            foreach (Monster enemy in enemies)
            {
                var enemyPos = enemy.transform.position;
                    
                if (enemyPos.Distance(source.transform.position) < 2)
                {                   
                    var angle = start.GetAngleBetween2D(enemyPos, end);
                    if(angle > 20)
                    {
                        continue;
                    }

                    var dir = enemy.transform.position - source.transform.position;
                    dir.y = 0;

                    enemy.transform.GetComponent<Rigidbody>().AddForce(50 * dir);

                    enemy.TakeDamage(source, damage);

                }
            }

            source.anim.SetTrigger("Punch");
            source.SetLastAttackTime(GameManager.time);
            source.LookAt(end);

            return true;
        }

        return false;
    }
}