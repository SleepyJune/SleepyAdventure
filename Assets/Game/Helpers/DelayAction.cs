using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class DelayAction
{
    public delegate void Callback();
    public static List<Action> actions = new List<Action>();

    static DelayAction()
    {

    }

    public static void OnUpdate()
    {
        for (var i = actions.Count - 1; i >= 0; i--)
        {
            if (actions[i].time <= Time.time)
            {
                if (actions[i].callback != null)
                {
                    actions[i].callback();
                }
                actions.RemoveAt(i);
            }
        }
    }

    public static void Add(Callback func, float time)
    {
        var action = new Action(time, func);
        actions.Add(action);
    }

    public struct Action
    {
        public Callback callback;
        public float time;

        public Action(float time, Callback callback)
        {
            this.time = time + Time.time;
            this.callback = callback;
        }
    }
}