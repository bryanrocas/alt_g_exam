using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner> 
{
	Vector3 initPos = new Vector3( -2.55f , 5.7f, 1f ) ;
	float xOffset = 1.0f ;
	float yOffset = 1.0f ;

	int index = 0;
	List<Sprite> sprites = new List<Sprite>() ;

	void Awake()
	{
		if( sprites.Count == 0 ) sprites = new List<Sprite>(Resources.LoadAll<Sprite>( "color_ball_full" )) ;
	}

	void Start()
	{
		// spawn objects based on X and Y

		for( int x = 0 ; x < Game.Manager.horBubbles ; x++ )
		{
			for( int y = 0 ; y < Game.Manager.verBubbles ; y++ )
			{
				int bubbleColor = Random.Range( 0, System.Enum.GetValues(typeof(BubbleColor)).Length ); ;
				GameObject bubble = InstantiateBubble( new Vector3( initPos.x + (xOffset * x) , initPos.y - (yOffset * y) , initPos.z ) , transform.rotation , (BubbleColor) bubbleColor ) ;
			}
		}

		BubblePool.Manager.InitRegistry();
	}

	public GameObject InstantiateBubble( Vector3 position, Quaternion rotation, BubbleColor bubbleColor )
	{
		GameObject bubble = Instantiate(Game.Manager.bubblePrefab, position, rotation) as GameObject;
		bubble.GetComponent<SpriteRenderer>().sprite = FindSprite( bubbleColor );
		bubble.AddComponent<Bubble>().Init( index , bubbleColor ) ;

		index++;

		return bubble ;
	}

	public Sprite FindSprite( BubbleColor bubbleColor )
	{
		return sprites.Find( obj => obj.name == (bubbleColor).ToString() ) ;
	}
}
