using UnityEngine;
using System.Collections;

public class JoystickController : MonoBehaviour {

	private float turnSpeed = 50f;
	private Vector2 movement;
	
	void Update()
	{
		Vector2 currentPosition = transform.position;
		Vector2 mousePos = new Vector3();
		bool mousePosAssigned = false;

		foreach (Touch touch in Input.touches)
		{
			if (touch.position.x < Screen.width/2)
			{
				mousePos = touch.position;
				mousePosAssigned = true;
			}
		}

		if (mousePosAssigned){
			//Touch touch = Input.GetTouch(0);


			//if (touch.phase == TouchPhase.Moved)
			if (Input.GetButton ("Fire1"))
			{
				//Vector2 moveTowards = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				movement = mousePos - currentPosition;
				movement.Normalize();
			}
		}
		
		float targetAngle = -(Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90);
		Debug.Log (targetAngle);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);
	}

}
