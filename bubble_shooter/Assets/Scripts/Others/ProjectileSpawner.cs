using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour 
{
	[SerializeField] SpriteRenderer loadedBubble;

	void OnEnable()
	{
		AimController.FireBubble += InstantiateBubble;
		AimController.ChangeBubble += ChangeAmmo;

		ChangeAmmo();
	}

	void OnDisable()
	{
		AimController.FireBubble -= InstantiateBubble;
		AimController.ChangeBubble -= ChangeAmmo;
	}

	int index = 0 ;
	int bubbleColor = 0 ;

	void InstantiateBubble()
	{
		GameObject bubble = Spawner.Manager.InstantiateBubble( transform.position , transform.rotation , (BubbleColor) bubbleColor ); 
		bubble.AddComponent<ProjectileController>().Init();
	}
		
	void ChangeAmmo()
	{
		if( bubbleColor >= System.Enum.GetValues(typeof(BubbleColor)).Length-1 ) bubbleColor = 0;
		else bubbleColor++ ;
		//Debug.LogError( (BubbleColor) bubbleColor + " " + bubbleColor  ) ;
		loadedBubble.sprite = Spawner.Manager.FindSprite( (BubbleColor) bubbleColor );

	}
}
