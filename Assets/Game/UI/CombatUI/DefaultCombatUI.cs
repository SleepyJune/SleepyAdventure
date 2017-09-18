using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultCombatUI : CombatUI
{
    int attackButtonTouchId = -2; //the finger id that activated the attack button
    int currentTouchId = -2;
    bool onAttackStart = false;
    
    int attackId = 0;

    // Use this for initialization
    public override void Initialize(AttackButton button)
    {
        attackButton = button;

        onAttackButtonDown = OnAttackButtonDownFunction;
        onAttackButtonUp = OnAttackButtonUpFunction;

        TouchInputManager.instance.touchStart += OnTouchStart;
        //TouchInputManager.instance.touchMove += OnTouchMove;
        //TouchInputManager.instance.touchEnd += OnTouchEnd;
    }

    public override void Death()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        //TouchInputManager.instance.touchMove -= OnTouchMove;
        //TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }

    void OnTouchStart(Touch touch)
    {     
        if (attackButton.isPressed && attackButtonTouchId != touch.fingerId)
        {
            Debug.Log("attac: " + touch.fingerId);

            //Debug.Log("attack");

            currentTouchId = touch.fingerId;

            var pos = GameManager.instance.GetTouchPosition(touch.position);
            if (pos != Vector3.zero)
            {
                Inventory.instance.equipment.weapon.Attack(
                    GameManager.instance.player,
                    GameManager.instance.player.transform.position,
                    pos);

            }
        }
    }

    void OnTouchMove(Touch touch)
    {

    }

    void OnTouchEnd(Touch touch)
    {

    }

    void OnAttackButtonDownFunction(PointerEventData eventData)
    {
        attackButtonTouchId = eventData != null ? eventData.pointerId : -2;

        //Debug.Log(attackButtonTouchId);

        //DelayAction.Add(() => StopAttack(attackId), 2); //retrieve this before firing?

        attackId += 1;
    }

    void OnAttackButtonUpFunction(PointerEventData eventData)
    {
        //StopAttack(attackId);
    }

    void StopAttack(int lastAttackId)
    {
        if(attackId == lastAttackId)
        {
            onAttackStart = false;
            currentTouchId = -2;
        }
    }

    // Update is called once per frame
    public override void Update()
    {

    }
}
