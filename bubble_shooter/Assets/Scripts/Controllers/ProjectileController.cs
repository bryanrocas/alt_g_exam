using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour 
{
	float speed 
	{
		get
		{
			return Game.Manager.projectileSpeed;
		}
	}

	Rigidbody2D rigidbdy ;
	Bubble bubble ;

	public void Init () 
	{
		rigidbdy = GetComponent<Rigidbody2D>() ;
		bubble = GetComponent<Bubble>() ;

		Vector3 force = transform.up.normalized * speed ;
		rigidbdy.AddForce( new Vector2(force.x,force.y), ForceMode2D.Force ) ;
		isMoving = true ;
	}

	bool isMoving = false;
	Vector3 lastPos = new Vector3() ;

	void Update()
	{
		if( isMoving )  lastPos = transform.position ;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		//this.DebugError( coll.gameObject.layer ) ;
		if( coll.gameObject.layer == LayerMask.NameToLayer("Bubble") )
		{
			isMoving = false;

			Vector2 contact = coll.contacts[0].point ;
			Vector3 dir = new Vector3(contact.x, contact.y, 1f) - lastPos;
			dir = -dir.normalized;
			transform.position = new Vector3(coll.transform.position.x + dir.x, coll.transform.position.y + dir.y, 1f)  ;

			rigidbdy.constraints = RigidbodyConstraints2D.FreezeAll ;
			bubble.RegisterSelf() ;

			Destroy( this ); // we remove the projectile controller attached to this object
		}
	}
}
