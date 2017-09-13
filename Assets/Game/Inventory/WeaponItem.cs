using UnityEngine;

public class WeaponItem : Item
{
    public Weapon weapon;
        
    public override bool Use()
    {        
        return Inventory.instance.equipment.ChangeWeapon(this);
    }
}