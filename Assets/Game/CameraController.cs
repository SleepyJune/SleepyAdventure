using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public float panSensitivity = .01f;

    bool isPanOn = false;

    Vector3 lastPanMousePosition;

    void ZoomFunction()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ZoomFunction();
    }
}
