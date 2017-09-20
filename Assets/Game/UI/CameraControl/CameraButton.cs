using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    Scene activeScene;

    Vector3 lastPanMousePosition;

    CameraFollow cameraFollowScript;

    void Start()
    {
        button = GetComponent<Button>();
        activeScene = SceneManager.GetActiveScene();

        cameraFollowScript = Camera.main.GetComponent<CameraFollow>();

        Initialize();
    }

    void Initialize()
    {
        inputs = TouchInputManager.instance.inputs;

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

    void Update()
    {
        if (TouchInputManager.instance.useMouse)
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

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                var scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
                ZoomFunction(scrollSpeed);
            }

            if (activeScene.name == "LevelLoader")
            {
                //CheckPan();
            }
        }
    }

    void CheckPan()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            lastPanMousePosition = Input.mousePosition;
        }

        if (Input.GetButton("Fire2"))
        {
            Vector2 delta = lastPanMousePosition - Input.mousePosition;
            Camera.main.transform.Translate(delta.x * panSensitivity, 0, delta.y * panSensitivity, Space.World);

            cameraFollowScript.isFollowing = false;

            lastPanMousePosition = Input.mousePosition;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            cameraFollowScript.isFollowing = true;
        }
    }

    void OnTouchStart(Touch touch)
    {
        if (isPressed && inputs.Count == panFingerCount)
        {
            TouchInput panTouch = inputs.Values.First(i => i.id != fingerId);
        }
    }

    void OnTouchMove(Touch touch)
    {
        if(touch.fingerId == -2 && activeScene.name == "LevelLoader")
        {
            Vector2 delta = touch.deltaPosition - touch.position;
            Camera.main.transform.Translate(delta.x * panSensitivity, 0, delta.y * panSensitivity, Space.World);
            cameraFollowScript.isFollowing = false;
            return;
        }

        if (isPressed && inputs.Count == panFingerCount) //pan function
        {
            TouchInput panTouch = inputs.Values.First(i => i.id != fingerId);

            Vector2 delta = panTouch.previousPosition - panTouch.position;
            Camera.main.transform.Translate(delta.x * panSensitivity, 0, delta.y * panSensitivity, Space.World);
            return;          
        }

        /*if (isPressed && inputs.Count >= zoomFingerCount) //zoom function
        {
            TouchInput zoomTouch1 = inputs.Values.First(i => i.id != fingerId);
            TouchInput zoomTouch2 = inputs.Values.First(i => i.id != fingerId && i.id != zoomTouch1.id);

            float prevTouchDeltaMag = (zoomTouch1.previousPosition - zoomTouch2.previousPosition).magnitude;
            float touchDeltaMag = (zoomTouch1.position - zoomTouch2.position).magnitude;
            
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ZoomFunction(-deltaMagnitudeDiff);
        }*/
    }

    void OnTouchEnd(Touch touch)
    {
        if(touch.fingerId == -2 && activeScene.name == "LevelLoader")
        {
            cameraFollowScript.isFollowing = true;
        }
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

    public void ZoomFunction(float scrollSpeed)
    {
        float fov = Camera.main.fieldOfView;
        fov -= scrollSpeed * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}