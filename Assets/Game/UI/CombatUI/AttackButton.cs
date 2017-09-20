using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    Weapon weapon;
    Button button;

    [NonSerialized]
    public bool isPressed = false;

    [NonSerialized]
    public int fingerId = (int)KeyCode.F;

    PlayerMovement player;
    bool usedInteractable = false;

    void Start()
    {
        weapon = Inventory.instance.equipment.weapon;
        button = GetComponent<Button>();

        Initialize();
    }

    void Initialize()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = (int)KeyCode.F;
            pointer.position = transform.position;

            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = (int)KeyCode.F;
            pointer.position = transform.position;

            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
        }
    }

    public void OnChangeWeapon(Weapon newWeapon)
    {
        if (weapon && weapon.combatUI)
        {
            weapon.combatUI.Death(); //deregister all listeners
        }

        weapon = newWeapon;

        if (newWeapon.combatUI)
        {
            newWeapon.combatUI.Initialize(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        isPressed = true;
        fingerId = eventData.pointerId;

        if (weapon && weapon.combatUI.onAttackButtonDown != null)
        {
            if (CheckInteractables())
            {
                usedInteractable = true;
            }
            else
            {
                weapon.combatUI.onAttackButtonDown(eventData);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (weapon && weapon.combatUI.onAttackButtonUp != null)
        {
            if (usedInteractable)
            {
                usedInteractable = false;
            }
            else
            {
                weapon.combatUI.onAttackButtonUp(eventData);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;

        if (weapon && weapon.combatUI.onAttackButtonUp != null)
        {
            if (usedInteractable)
            {
                usedInteractable = false;
            }
            else
            {
                weapon.combatUI.onAttackButtonUp(eventData);
            }
        }
    }

    bool CheckInteractables()
    {
        var playerForward = player.pos + player.transform.forward;

        return GameManager.instance.UseInteractable(player, playerForward);
    }
}