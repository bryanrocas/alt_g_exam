using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour 
{
	[SerializeField] GameObject bubblePrefab ;

	void OnEnable()
	{
		AimController.FireBubble += InstantiateBubble;
	}

	void OnDisable()
	{
		AimController.FireBubble -= InstantiateBubble;
	}

	void InstantiateBubble( Vector3 direction )
	{
		this.Debug( "FIRE: " + direction );

		GameObject bullet = Instantiate(bubblePrefab, direction, Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody2D>().velocity = direction ;
	}
}
