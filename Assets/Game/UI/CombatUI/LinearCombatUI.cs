using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class LinearCombatUI : CombatUI
{
    public GameObject linePrefab;

    LineRenderer line;

    Vector2 touchStart;

    PlayerMovement player;
        
    int currentTouchId = -2;

    // Use this for initialization
    public override void Initialize(AttackButton button)
    {
        attackButton = button;

        player = GameManager.instance.player;
        onAttackButtonDown = OnAttackButtonDownFunction;
        onAttackButtonUp = OnAttackButtonUpFunction;

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

    void OnTouchStart(Touch touch)
    {
        if (attackButton.isPressed && currentTouchId != touch.fingerId && attackButton.fingerId != touch.fingerId)
        {            
            if(line)
            {
                Destroy(line.gameObject); //destroy the previous line                
            }

            line = Instantiate(linePrefab).GetComponent<LineRenderer>();
            line.SetPosition(0, player.transform.position);
            
            touchStart = touch.position;

            currentTouchId = touch.fingerId;
        }
    }

    void OnTouchMove(Touch touch)
    {
        if (attackButton.isPressed && line && touch.fingerId == currentTouchId)
        {            
            Vector3 diff2d = (touch.position - touchStart).normalized;
            Vector3 diff = new Vector3(diff2d.x, 0, diff2d.y);

            if (diff == Vector3.zero)
            {
                diff = player.transform.forward;
            }
                        
            Vector3 pos = player.transform.position + diff * 2;

            line.SetPosition(1, pos);
        }
    }

    void OnTouchEnd(Touch touch)
    {
        if (attackButton.isPressed && line && touch.fingerId == currentTouchId)
        {
            Vector3 diff2d = (touch.position - touchStart).normalized;
            Vector3 diff = new Vector3(diff2d.x, 0, diff2d.y);

            if (diff == Vector3.zero)
            {
                diff = player.transform.forward;
            }

            Vector3 pos = player.transform.position + diff * 2;

            Inventory.instance.equipment.weapon.Attack(
                    GameManager.instance.player,
                    GameManager.instance.player.transform.position,
                    pos);

            if (line.gameObject != null)
            {
                Destroy(line.gameObject);
                line = null;
            }

            currentTouchId = -2;
        }
    }

    void OnAttackButtonDownFunction(PointerEventData eventData)
    {
        
    }

    void OnAttackButtonUpFunction(PointerEventData eventData)
    {
        if (line != null)
        {
            if (currentTouchId >= 0)
            {
                var touch = Input.touches[currentTouchId];
                if (touch.phase != TouchPhase.Ended || touch.phase != TouchPhase.Canceled)
                {
                    OnTouchEnd(touch);
                }
            }
            else
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Ended;

                OnTouchEnd(touchData);
            }
        }

        currentTouchId = -2;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (player)
        { 
            if (line)
            {
                line.SetPosition(0, player.transform.position); //line follow player, maybe move to player transform
            }
        }
    }
}
