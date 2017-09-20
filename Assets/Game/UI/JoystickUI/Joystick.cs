using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnitySampleAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int MovementRange = 100;

    public enum AxisOption
    {                                                    // Options for which axes to use                                                     
        Both,                                                                   // Use both
        OnlyHorizontal,                                                         // Only horizontal
        OnlyVertical                                                            // Only vertical
    }

    public AxisOption axesToUse = AxisOption.Both;   // The options for the axes that the still will use
    public string horizontalAxisName = "Horizontal";// The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical";    // The name given to the vertical axis for the cross platform input 

    private Vector3 startPos;
    private bool useX;                                                          // Toggle for using the x axis
    private bool useY;                                                          // Toggle for using the Y axis
    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;               // Reference to the joystick in the cross platform input
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;                 // Reference to the joystick in the cross platform input

    [System.NonSerialized]
    public int fingerId = 999;

    enum DragState
    {
        Begin,
        Drag,
        End
    }

    private DragState dragState = DragState.End;

    public float xAxis
    {
        get
        {
            return horizontalVirtualAxis.GetValue;
        }
    }

    public float yAxis
    {
        get
        {
            return verticalVirtualAxis.GetValue;
        }
    }

    void OnEnable()
    {
        startPos = transform.position;
        CreateVirtualAxes();
    }

    void Update()
    {
        float h = 0;
        float v = 0;
        
        if (Input.GetKey(KeyCode.W))
        {
            v = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            h = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            v = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            h = 1;
        }

        //verticalVirtualAxis.Update(v);
        //horizontalVirtualAxis.Update(h);              

        if(h != 0 || v != 0)
        {
            var pos = startPos
                + Vector3.right * MovementRange * h + Vector3.up * MovementRange * v;

            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = (int)KeyCode.Space;
            pointer.position = 
                new Vector3(
                    Mathf.Lerp(transform.position.x, pos.x, .1f),
                    Mathf.Lerp(transform.position.y, pos.y, .1f), 0);
                                
            if(dragState == DragState.End)
            {
                pointer.position = transform.position;

                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.beginDragHandler);
                //ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.dragHandler);

                dragState = DragState.Drag;
            }
            else if (dragState == DragState.Drag)
            {
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.dragHandler);
                dragState = DragState.Drag;
            }

            //ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerDownHandler);
        }
        else
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.pointerId = -1;
            pointer.position = transform.position;

            if (dragState == DragState.Drag)
            {
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.endDragHandler);
                dragState = DragState.End;
            }
        }

    }

    private void UpdateVirtualAxes(Vector3 value)
    {
        var delta = startPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;
        if (useX)
        {
            horizontalVirtualAxis.Update(-delta.x);
        }

        if (useY)
        {
            verticalVirtualAxis.Update(delta.y);
        }
    }

    private void CreateVirtualAxes()
    {
        // set axes to use
        useX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        useY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        // create new axes based on axes to use
        if (useX)
        {
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);            
        }
            
        if (useY)
        {
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        }
    }
       

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;

        fingerId = eventData.pointerId;
    }

    public void OnDrag(PointerEventData data)
    {
        //Debug.Log(data.position);

        Vector3 newPos = Vector3.zero;

        if (useX)
        {
            int delta = (int)(data.position.x - startPos.x);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.x = delta;
        }

        if (useY)
        {
            int delta = (int)(data.position.y - startPos.y);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.y = delta;
        }
        transform.position = new Vector3(startPos.x + newPos.x, startPos.y + newPos.y, startPos.z + newPos.z);
        UpdateVirtualAxes(transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos;
        UpdateVirtualAxes(startPos);

        fingerId = 999;
    }

    void OnDisable()
    {
        // remove the joysticks from the cross platform input
        if (useX)
        {
            horizontalVirtualAxis.Remove();
        }
        if (useY)
        {
            verticalVirtualAxis.Remove();
        }
    }
}
