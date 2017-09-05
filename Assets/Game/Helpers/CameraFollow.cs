using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    [System.NonSerialized]
    public Transform target;

    float smoothing = .3f; //.5 means midway between frame a and b, the lower the number, the more smoothing

    public float fov = 30;
    private float previousfov;

    public bool isFollowing = false;

    void Start()
    {
        if (GameManager.instance.player != null)
        {
            target = GameManager.instance.player.transform;
        }
    }

    void FixedUpdate()
    {
        if(target == null)
        {
            if(GameManager.instance.player != null)
            {
                target = GameManager.instance.player.transform;
            }
        }

        if (isFollowing && target != null)
        {
            if (Camera.main.fieldOfView != fov)
            {
                previousfov = Camera.main.fieldOfView;
                Camera.main.fieldOfView = fov;
            }

            Vector3 targetCameraPos = target.Find("FrontCameraPos").transform.position;
            var targetCameraRotation = target.Find("FrontCameraPos").transform.rotation;
            
            transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraRotation, smoothing);
        }
    }
}
