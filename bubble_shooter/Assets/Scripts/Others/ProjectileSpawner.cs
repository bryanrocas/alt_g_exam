using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour 
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

	void InstantiateBubble()
	{
		GameObject bubble = Instantiate(bubblePrefab, transform.position, transform.rotation) as GameObject;
		bubble.AddComponent<ProjectileController>();
	}
}
