using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour 
{
	float rotateZ = 0;

	[SerializeField] float minClamp = -80f;
	[SerializeField] float maxClamp = 80f;
	[SerializeField] float rotateSpeed = 2f;


	public delegate void FireProjectileDg( Vector3 direction );
	public static event FireProjectileDg FireBubble;


	void Update()
	{
		#if DEBUG
		Vector3 forward = transform.TransformDirection(Vector3.up) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);
		#endif

		RotateAim();


		if( Input.GetKeyUp(KeyCode.Space) )
		{
			if( FireBubble != null ) FireBubble( transform.TransformDirection(Vector3.up) ) ; 
		}
	}

	void RotateAim()
	{
		rotateZ += Input.GetAxis("Horizontal") * rotateSpeed;
		rotateZ = Mathf.Clamp( rotateZ , minClamp , maxClamp );

		transform.localEulerAngles = new Vector3( transform.localEulerAngles.x,
			transform.localEulerAngles.y,
			-rotateZ);
	}
}
