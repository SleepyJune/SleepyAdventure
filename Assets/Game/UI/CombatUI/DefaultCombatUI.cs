using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultCombatUI : CombatUI
{    
    int currentTouchId = -2;

    bool isAttacking = false;

    int attackTouchId = 0;

    Vector3 attackPos;

    PlayerMovement player;

    Joystick joystick;

    // Use this for initialization
    public override void Initialize(AttackButton button)
    {
        player = GameManager.instance.player;
        joystick = GameManager.instance.joystick;

        isAttacking = false;

        TouchInputManager.instance.touchStart += OnTouchStart;
        TouchInputManager.instance.touchMove += OnTouchMove;
        TouchInputManager.instance.touchEnd += OnTouchEnd;
    }

    public override void Death()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
    
    // Update is called once per frame
    public override void Update()
    {
        if (isAttacking)
        {
            Inventory.instance.equipment.weapon.Attack(
                    GameManager.instance.player,
                    GameManager.instance.player.transform.position,
                    attackPos);
        }
    }

    void OnTouchStart(Touch touch)
    {        
        if (isAttacking == false 
            && joystick.fingerId != touch.fingerId 
            && !touch.IsPointerOverUI() 
            && touch.fingerId != -2)
        {
            if (!CheckInteractables())
            {
                isAttacking = true;

                //Debug.Log("attac: " + touch.fingerId);

                attackPos = GameManager.instance.GetTouchPosition(touch.position);
                if(attackPos == Vector3.zero)
                {
                    attackPos = player.transform.forward;
                }
                                
                currentTouchId = touch.fingerId;                
            }
        }
    }

    void OnTouchMove(Touch touch)
    {
        if (isAttacking && touch.fingerId == currentTouchId)
        {
            attackPos = GameManager.instance.GetTouchPosition(touch.position);
            if (attackPos == Vector3.zero)
            {
                attackPos = player.transform.forward;
            }
        }
    }

    void OnTouchEnd(Touch touch)
    {
        if(isAttacking && touch.fingerId == currentTouchId)
        {
            isAttacking = false;
        }
    }

    bool CheckInteractables()
    {
        var playerForward = player.pos + player.transform.forward;

        return GameManager.instance.UseInteractable(player, playerForward);
    }
}
