using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager instance = null;

    public Dictionary<int, TouchInput> inputs;

    public delegate void Callback(Touch touch);
    public event Callback touchStart;
    public event Callback touchMove;
    public event Callback touchEnd;

    public bool useMouse = false;

    Vector3 lastMousePosition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Application.isMobilePlatform)
        {
            useMouse = false;
        }

        if (!useMouse)
        {
            Input.simulateMouseWithTouches = false;
        }

        inputs = new Dictionary<int, TouchInput>();
    }

    void Update()
    {
        if (useMouse && Input.mousePresent)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Began;
                
                lastMousePosition = Input.mousePosition;

                TouchInput newInput;
                if (inputs.TryGetValue(touchData.fingerId, out newInput))
                {
                    newInput.startPosition = touchData.position;
                }
                else
                {
                    newInput = new TouchInput(touchData);
                    inputs.Add(newInput.id, newInput);
                }

                if (touchStart != null)
                {
                    touchStart(touchData);
                }
            }

            if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Moved;

                UpdateTouchPosition(touchData.fingerId, touchData.position);

                lastMousePosition = Input.mousePosition;

                if (touchMove != null)
                {
                    touchMove(touchData);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Ended;

                UpdateTouchPosition(touchData.fingerId, touchData.position);

                lastMousePosition = Input.mousePosition;

                if (touchEnd != null)
                {
                    touchEnd(touchData);
                }

                inputs.Remove(touchData.fingerId);
            }
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    TouchInput newInput;
                    if (inputs.TryGetValue(touch.fingerId, out newInput))
                    {
                        newInput.startPosition = touch.position;
                    }
                    else
                    {
                        newInput = new TouchInput(touch);
                        inputs.Add(newInput.id, newInput);
                    }

                    if (touchStart != null)
                    {
                        touchStart(touch);
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    UpdateTouchPosition(touch.fingerId, touch.position);

                    if (touchMove != null)
                    {
                        touchMove(touch);
                    }
                }

                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    UpdateTouchPosition(touch.fingerId, touch.position);

                    if (touchEnd != null)
                    {
                        touchEnd(touch);
                    }

                    inputs.Remove(touch.fingerId);
                }
            }
        }
    }

    void UpdateTouchPosition(int fingerId, Vector2 position)
    {
        TouchInput newInput;
        if (inputs.TryGetValue(fingerId, out newInput))
        {
            newInput.previousPosition = newInput.position;
            newInput.position = position;
        }
    }
}
