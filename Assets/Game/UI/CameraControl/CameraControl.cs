using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 0.05f;
    public float panSensitivity = 0.01f;

    //public bool noButton = false;

    [NonSerialized]
    public bool isPressed = false;
    
    Dictionary<int, TouchInput> inputs;
    Button button;

    int panFingerCount = 1;
    int zoomFingerCount = 3;
        
    Vector3 lastPanMousePosition;

    int panFingerID = -1;

    CameraFollow cameraFollowScript;

    AttackButton attackButton;

    void Start()
    {
        button = GetComponent<Button>();

        cameraFollowScript = GetComponent<CameraFollow>();

        Initialize();
    }

    void Initialize()
    {
        inputs = TouchInputManager.instance.inputs;

        attackButton = GameManager.instance.attackButton;

        TouchInputManager.instance.touchStart += OnTouchStart;
        TouchInputManager.instance.touchMove += OnTouchMove;
        TouchInputManager.instance.touchEnd += OnTouchEnd;

        if (TouchInputManager.instance.useMouse) //temporary
        {
            panFingerCount = 1;
            panSensitivity = 0.02f;
            sensitivity = 10;
        }
    }

    void OnDestroy()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
        
    void OnTouchStart(Touch touch)
    {
        if (!touch.IsPointerOverUI())
        {
            //TouchInput panTouch = inputs.Values.First();
        }
    }

    void OnTouchMove(Touch touch)
    {
        if (!attackButton.isPressed && !touch.IsPointerOverUI()) //pan function
        {            
            Vector2 delta = touch.deltaPosition - touch.position;
            Camera.main.transform.Translate(delta.x * panSensitivity, 0, delta.y * panSensitivity, Space.World);

            cameraFollowScript.isFollowing = false;
        }
    }

    void OnTouchEnd(Touch touch)
    {
        cameraFollowScript.isFollowing = true;
    }
}