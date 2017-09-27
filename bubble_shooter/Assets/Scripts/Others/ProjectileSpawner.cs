using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour 
{
	[SerializeField] GameObject bubblePrefab ;
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
	int bubbleColor ;
	Sprite[] sprites ;

	void InstantiateBubble()
	{
		GameObject bubble = Instantiate(bubblePrefab, transform.position, transform.rotation) as GameObject;
		bubble.GetComponent<SpriteRenderer>().sprite = sprites[bubbleColor] ;
		bubble.AddComponent<Bubble>().Init( index , (BubbleColor) bubbleColor ) ;
		bubble.AddComponent<ProjectileController>();

		index++;
	}
		
	void ChangeAmmo()
	{
		bubbleColor = Random.Range( 0, System.Enum.GetValues(typeof(BubbleColor)).Length );

		if( sprites == null ) sprites = Resources.LoadAll<Sprite>( loadedBubble.sprite.texture.name );

		loadedBubble.sprite = sprites[bubbleColor];

	}
}
