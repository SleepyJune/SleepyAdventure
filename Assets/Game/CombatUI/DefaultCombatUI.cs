using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCombatUI : CombatUI
{    
    // Use this for initialization
    public override void Initialize()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKey("space"))
        {
            Inventory.instance.equipment.weapon.Attack(GameManager.instance.player, Vector3.zero, Vector3.zero);
        }
    }
}
