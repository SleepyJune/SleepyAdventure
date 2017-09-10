
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Weapon weapon;

    public bool ChangeWeapon(Weapon newWeapon)
    {
        if (this.weapon != newWeapon)
        {
            this.weapon = newWeapon;
            return true;
        }
        else
        {
            this.weapon = null;
            return false;
        }

        return false;
    }
}