
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public WeaponItem currentWeaponItem;

    public Weapon weapon;
    public Weapon defaultWeapon;

    public Color equipedBackground;
    public Color normalBackground;

    void Awake()
    {
        weapon = defaultWeapon;
    }

    public bool ChangeWeapon(WeaponItem weaponItem)
    {
        var newWeapon = weaponItem.weapon;

        if (weapon != newWeapon)
        {
            if (currentWeaponItem)
            {
                currentWeaponItem.itemSlot.background.color = normalBackground;
            }

            weaponItem.itemSlot.background.color = equipedBackground;

            weapon = newWeapon;
            currentWeaponItem = weaponItem;

            GameManager.instance.player.OnChangeWeapon(newWeapon);
            return true;
        }
        else
        {
            currentWeaponItem.itemSlot.background.color = normalBackground;
            currentWeaponItem = null;

            weapon = defaultWeapon;
            GameManager.instance.player.OnChangeWeapon(defaultWeapon);
            return false;
        }
    }
}