using UnityEngine;

public class Explosion : IdleProjectile
{
    void OnTriggerEnter(Collider collision)
    {
        var unit = collision.gameObject.GetComponent<AttackableUnit>();
        if (unit != null)
        {
            Debug.Log(unit.name);
            unit.TakeDamage(source, damage);
            destroyed = true;
            Destroy(gameObject, 2);
        }
    }
}