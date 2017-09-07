using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class CameraController : MonoBehaviour {

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public float panSensitivity = .01f;

    bool isPanOn = false;

    Vector3 lastPanMousePosition;

    Vector3 playerCameraPos;
    Transform player;

    public float smooth = 3f;

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

    void PanFunction()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            lastPanMousePosition = Input.mousePosition;
        }

        if (Input.GetButton("Fire2"))
        {
            Vector3 delta = -Input.mousePosition + lastPanMousePosition;
            Camera.main.transform.Translate(delta.x * panSensitivity, delta.y * panSensitivity, 0);
            lastPanMousePosition = Input.mousePosition;
        }

    }

    void FollowPlayer()
    {
        if (player == null)
        {
            var playerObj = GameObject.FindWithTag("Player");

            if (playerObj == null) return; //if still can't find player

            player = playerObj.transform;
            playerCameraPos = Camera.main.transform.position;
        }

        var camera = Camera.main.transform;

        camera.position = Vector3.Lerp(camera.position, player.position + playerCameraPos, Time.fixedDeltaTime * smooth);
        //camera.forward = Vector3.Lerp(camera.forward, playerCameraPos.forward, Time.fixedDeltaTime * smooth);

    }

	// Update is called once per frame
	void Update () {
        ZoomFunction();
        PanFunction();
        //FollowPlayer();
    }
}
