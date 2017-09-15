using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour
{
    public delegate void Callback(Touch touch);
    public event Callback touchStart;
    public event Callback touchMove;
    public event Callback touchEnd;

    public bool useMouse = false;

    Vector3 lastMousePosition;

    void Awake()
    {
        if (!useMouse)
        {
            Input.simulateMouseWithTouches = false;
        }
    }

    void Update()
    {
        if (useMouse && Input.mousePresent)
        {
            if (touchStart != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var touchData = new Touch();
                    touchData.fingerId = -1;
                    touchData.position = Input.mousePosition;
                    touchData.phase = TouchPhase.Began;

                    touchStart(touchData);

                    lastMousePosition = Input.mousePosition;
                }
            }

            if (touchMove != null)
            {
                if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
                {
                    var touchData = new Touch();
                    touchData.fingerId = -1;
                    touchData.position = Input.mousePosition;
                    touchData.phase = TouchPhase.Moved;

                    touchMove(touchData);

                    lastMousePosition = Input.mousePosition;
                }
            }

            if (touchEnd != null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    var touchData = new Touch();
                    touchData.fingerId = -1;
                    touchData.position = Input.mousePosition;
                    touchData.phase = TouchPhase.Ended;

                    touchEnd(touchData);

                    lastMousePosition = Input.mousePosition;
                }
            }
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touchStart != null)
                {
                    if (touch.phase == TouchPhase.Began)
                    {

                        touchStart(touch);

                    }
                }

                if (touchMove != null)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {

                        touchMove(touch);

                    }
                }

                if (touchEnd != null)
                {
                    if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {

                        touchEnd(touch);
                    }
                }                
            }
        }
    }
}
