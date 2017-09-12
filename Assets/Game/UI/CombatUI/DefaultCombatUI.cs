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
            var pos = GameManager.instance.GetMousePosition();
            if (pos != Vector3.zero)
            {
                Inventory.instance.equipment.weapon.Attack(
                    GameManager.instance.player,
                    GameManager.instance.player.transform.position,
                    pos);

            }
        }
    }
}
