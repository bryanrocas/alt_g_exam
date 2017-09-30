using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner> 
{
	Vector3 initPos = new Vector3( -4.5f ,9f, 1f ) ;
	float xOffset = 1.0f ;
	float yOffset = 1.0f ;

	int index = 0;
	List<Sprite> sprites = new List<Sprite>() ;

	void Start()
	{
		// spawn objects based on X and Y

		for( int x = 0 ; x < Game.Manager.horBubbles ; x++ )
		{
			for( int y = 0 ; y < Game.Manager.verBubbles ; y++ )
			{
				int rowFlag = y % 2;
				float adjustment = (rowFlag == 0) ? 0f : 0.5f;

				int bubbleColor = Random.Range( 0, System.Enum.GetValues(typeof(BubbleColor)).Length ); ;
				GameObject bubble = InstantiateBubble( new Vector3( initPos.x + (xOffset * x) + adjustment , initPos.y - (yOffset * y) , initPos.z ) , transform.rotation , (BubbleColor) bubbleColor ) ;
				bubble.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll ;
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
		if( sprites.Count == 0 ) sprites = new List<Sprite>(Resources.LoadAll<Sprite>( "color_ball_full" )) ;
		return sprites.Find( obj => obj.name == (bubbleColor).ToString() ) ;
	}
}
