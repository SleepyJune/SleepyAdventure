using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class WorldCameraController : MonoBehaviour
{
    public Transform cameraTransform;

    void Start()
    {
        if(cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(
            cameraTransform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z
            );
    }
}
