using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public CombatUI combatUI;

    public abstract bool Attack(AttackableUnit unit, Vector3 start, Vector3 end);
}