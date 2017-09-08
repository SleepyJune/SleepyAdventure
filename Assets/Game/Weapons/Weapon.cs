using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract bool Attack(AttackableUnit unit);
}