using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CombatUI : MonoBehaviour
{
    //protected AttackButton attackButton;

    public delegate void EventTrigger(PointerEventData data);
    
    public EventTrigger onAttackButtonDown;
    public EventTrigger onAttackButtonUp;

    public AttackButton attackButton;

    // Use this for initialization
    public abstract void Initialize(AttackButton button);

    // Update is called once per frame
    public abstract void Update();

    public abstract void Death();
}
