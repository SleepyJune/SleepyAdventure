
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Weapon weapon;

    public Weapon defaultWeapon;

    void Awake()
    {
        weapon = defaultWeapon;
    }

    public bool ChangeWeapon(Weapon newWeapon)
    {
        if (this.weapon != newWeapon)
        {
            this.weapon = newWeapon;                      
            GameManager.instance.player.OnChangeWeapon(newWeapon);
            return true;
        }
        else
        {
            this.weapon = defaultWeapon;
            GameManager.instance.player.OnChangeWeapon(defaultWeapon);
            return false;
        }
    }
}