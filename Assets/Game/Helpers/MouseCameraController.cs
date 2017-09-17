using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using System.Linq;

public class MouseCameraController : MonoBehaviour
{

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public float panSensitivity = .02f;

    bool isPanOn = false;

    Vector3 lastPanMousePosition;

    Vector3 playerCameraPos;
    Transform player;

    public float smooth = 3f;

    Scene activeScene;

    void Start()
    {
        activeScene = SceneManager.GetActiveScene();
    }

    public void ZoomFunction(float scrollSpeed)
    {
        float fov = Camera.main.fieldOfView;
        fov -= scrollSpeed * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    void CheckPan()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            lastPanMousePosition = Input.mousePosition;
        }

        if (Input.GetButton("Fire2"))
        {
            PanFunction(lastPanMousePosition, Input.mousePosition);

            lastPanMousePosition = Input.mousePosition;
        }
    }
        
    public void PanFunction(Vector3 pointerOrigin, Vector3 pointerCurrent)
    {
        Vector3 delta = pointerOrigin - pointerCurrent;
        Camera.main.transform.Translate(delta.x * panSensitivity, 0, delta.y * panSensitivity, Space.World);
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
    void Update()
    {
        if (!Application.isMobilePlatform)
        {            
            if (activeScene.name == "LevelLoader")
            {
                var scrollSpeed = Input.GetAxis("Mouse ScrollWheel");            
                ZoomFunction(scrollSpeed);
                CheckPan();
                //FollowPlayer();
            }
        }
    }
}
