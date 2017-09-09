using UnityEngine;

public class WeaponItem : Item
{
    public Weapon weapon;

    public override bool Use()
    {
        GameManager.instance.OnPlayerChangeWeapon(weapon);
        return true;
    }
}