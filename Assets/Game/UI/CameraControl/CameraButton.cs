using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 0.05f;
    public float panSensitivity = 0.01f;

    //public bool noButton = false;

    [NonSerialized]
    public bool isPressed = false;

    [NonSerialized]
    public int fingerId = (int)KeyCode.C;
    
    Dictionary<int, TouchInput> inputs;
    Button button;

    int panFingerCount = 2;
    int zoomFingerCount = 3;


    void Awake()
    {
        button = GetComponent<Button>();

        GameManager.instance.OnGameStart += Initialize;      
    }

    void Initialize()
    {
        inputs = GameManager.instance.inputManager.inputs;

        GameManager.instance.inputManager.touchStart += OnTouchStart;
        GameManager.instance.inputManager.touchMove += OnTouchMove;
        GameManager.instance.inputManager.touchEnd += OnTouchEnd;

        if (GameManager.instance.inputManager.useMouse)
        {
            panFingerCount = 1;
        }

        GameManager.instance.OnGameStart -= Initialize;
    }

    void OnDestroy()
    {
        GameManager.instance.inputManager.touchStart -= OnTouchStart;
        GameManager.instance.inputManager.touchMove -= OnTouchMove;
        GameManager.instance.inputManager.touchEnd -= OnTouchEnd;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = (int)KeyCode.C;
            pointer.position = transform.position;

            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = (int)KeyCode.C;
            pointer.position = transform.position;

            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
        }
    }

    void OnTouchStart(Touch touch)
    {
        if (isPressed && inputs.Count == panFingerCount)
        {
            TouchInput panTouch = inputs.Values.First(i => i.id != fingerId);
        }

        /*if (noButton && inputs.Count == panFingerCount) //pan function
        {
            isPressed = true;

            foreach (var input in inputs.Values)
            {
                if (Input.touches[input.id].IsPointerOverUI())
                {
                    isPressed = false;
                    break;
                }
            }
        }*/
    }

    void OnTouchMove(Touch touch)
    {
        if (isPressed && inputs.Count == panFingerCount) //pan function
        {
            TouchInput panTouch = inputs.Values.First(i => i.id != fingerId);

            Vector2 delta = panTouch.previousPosition - panTouch.position;
            Camera.main.transform.Translate(delta.x * panSensitivity, delta.y * panSensitivity, 0);            
        }

        if (isPressed && inputs.Count >= zoomFingerCount) //zoom function
        {
            TouchInput zoomTouch1 = inputs.Values.First(i => i.id != fingerId);
            TouchInput zoomTouch2 = inputs.Values.First(i => i.id != fingerId && i.id != zoomTouch1.id);

            float prevTouchDeltaMag = (zoomTouch1.previousPosition - zoomTouch2.previousPosition).magnitude;
            float touchDeltaMag = (zoomTouch1.position - zoomTouch2.position).magnitude;
            
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                        
            float fov = Camera.main.fieldOfView;
            fov += deltaMagnitudeDiff * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }

        /*if (noButton && inputs.Count == panFingerCount) //pan function
        {
            isPressed = true;

            foreach (var input in inputs.Values)
            {
                if (Input.touches[input.id].IsPointerOverUI())
                {
                    isPressed = false;
                    break;
                }
            }

            if (isPressed)
            {
                TouchInput panTouch1 = inputs.Values.First();
                TouchInput panTouch2 = inputs.Values.First(i => i.id != panTouch1.id);

                Vector2 delta1 = panTouch1.previousPosition - panTouch1.position;
                Vector2 delta2 = panTouch2.previousPosition - panTouch2.position;

                Vector2 delta = delta1 + delta2;

                Camera.main.transform.Translate(delta.x * panSensitivity, delta.y * panSensitivity, 0);
            }           
        }*/
    }

    void OnTouchEnd(Touch touch)
    {
        /*if (noButton && isPressed)
        {
            isPressed = false;
        }*/
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        fingerId = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
    }
}