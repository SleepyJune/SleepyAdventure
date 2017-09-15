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

    void Start()
    {
        weapon = Inventory.instance.equipment.weapon;
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = -2;
            pointer.position = transform.position;

            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = -2;
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
        //Debug.Log("mouseDown: " + eventData.pointerId);

        if (weapon && weapon.combatUI.onAttackButtonDown != null)
        {
            weapon.combatUI.onAttackButtonDown(eventData);
        }
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (weapon && weapon.combatUI.onAttackButtonUp != null)
        {
            weapon.combatUI.onAttackButtonUp(eventData);
        }
        isPressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (weapon && weapon.combatUI.onAttackButtonUp != null)
        {
            weapon.combatUI.onAttackButtonUp(eventData);
        }
        isPressed = false;
    }
}