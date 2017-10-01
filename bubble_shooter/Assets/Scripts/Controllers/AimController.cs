using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour 
{
	float rotateZ = 0;

	[SerializeField] float minClamp = -80f;
	[SerializeField] float maxClamp = 80f;
	[SerializeField] float rotateSpeed = 2f;
	[SerializeField] float offset = 270f;


	public delegate void FireProjectileDg();
	public static event FireProjectileDg FireBubble;

	public delegate void ChangeBubbleDg();
	public static event ChangeBubbleDg ChangeBubble;

	bool IsAimInBounds
	{
		get
		{
			return ( rotateZ > maxClamp || rotateZ < minClamp ) ? false : true;
		}
	}

	void Update()
	{
		#if DEBUGGER
		Vector3 forward = transform.TransformDirection(Vector3.up) * 10f;
		//this.Debug( forward );
		Debug.DrawRay(transform.position, forward, Color.green);
		#endif

		RotateAim();

		if( Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire1") && IsAimInBounds )
		{
			if( FireBubble != null ) FireBubble() ; 
			if( ChangeBubble != null ) ChangeBubble();
		}

		if(  Input.GetButtonUp("Fire2") )
		{
			if( ChangeBubble != null ) ChangeBubble();
		}
	}

	void RotateAim()
	{
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();
		rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
	}
}
