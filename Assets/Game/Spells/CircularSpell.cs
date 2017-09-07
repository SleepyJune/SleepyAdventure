using UnityEngine;

public class CircularSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;

    void Awake()
    {
        Initialize();
    }

    void OnTriggerEnter(Collider collision)
    {
        var unit = collision.gameObject.GetComponent<AttackableUnit>();
        if (unit != null)
        {
            unit.TakeDamage(source, damage);
            isDead = true;
            Destroy(gameObject, 2);
        }
    }
}