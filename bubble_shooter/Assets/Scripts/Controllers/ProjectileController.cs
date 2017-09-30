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


	public void Init () 
	{
		Vector3 force = transform.up.normalized * speed ;
		GetComponent<Rigidbody2D>().AddForce( new Vector2(force.x,force.y), ForceMode2D.Force ) ;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		//this.DebugError( coll.gameObject.layer ) ;
		if( coll.gameObject.layer == LayerMask.NameToLayer("Bubble") )
		{
			this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll ;
			this.GetComponent<Bubble>().RegisterNeighbors() ;
			this.GetComponent<Bubble>().MatchCheck() ;

			Destroy( this ); // we remove the projectile controller attached to this object
		}
	}
}
