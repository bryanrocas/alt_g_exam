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

	bool isMoving = true;

	void Update () 
	{
		if( isMoving ) transform.position += transform.up * speed * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		isMoving = false;

		this.GetComponent<Bubble>().RegisterNeighbors() ;

		Destroy( this ); // we remove the projectile controller attached to this object
	}
}
