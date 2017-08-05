using UnityEngine;
using System.Collections;

public class joystick : MonoBehaviour {

	private float turnSpeed = 50f;
	private Vector2 movement;
	
	void Update()
	{
		Vector2 currentPosition = transform.position;
		
		if (true)
		{
			//Touch touch = Input.GetTouch(0);
			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


			//if (touch.phase == TouchPhase.Moved)
			if (Input.GetButton ("Fire1"))
			{
				//Vector2 moveTowards = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				movement = mousePos - currentPosition;
				movement.Normalize();
			}
		}
		
		float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
		//Debug.Log (targetAngle);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);
	}
}
