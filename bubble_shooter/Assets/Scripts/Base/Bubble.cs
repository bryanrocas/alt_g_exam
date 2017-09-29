using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bubble : MonoBehaviour, IBubble, IBubbleDetect
{
	[SerializeField] int id;
	[SerializeField] BubbleColor bubbleColor;

	#region IBubble implementation
	public int ID 
	{
		get 
		{
			return id;
		}

		protected set
		{
			id = value;
		}
	}
	public BubbleColor BubbleColor 
	{
		get 
		{
			return bubbleColor;
		}

		protected set
		{
			bubbleColor = value;
		}
	}

	public void Init (int id, BubbleColor bubbleColor)
	{
		ID = id;
		BubbleColor = bubbleColor;

		BubblePool.Manager.RegisterBubbles( this ) ;
	}
	#endregion





	public List<IBubble> GetMatchingNeighbors ()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll( new Vector2(transform.position.x , transform.position.y) , Game.Manager.bubbleRadius , 9 );

		List<IBubble> matchingNeighbors = new List<IBubble>();

		foreach( Collider2D col in cols )
		{
			IBubble bubble = col.GetComponent<IBubble>() ;
			if( bubble.BubbleColor == BubbleColor ) matchingNeighbors.Add( bubble );
		}

		return matchingNeighbors;
	}

	public void RegisterNeighbors()
	{
		BubblePool.Manager.RegisterNeighbors( this ,GetMatchingNeighbors() );
	}

	public void MatchCheck()
	{
		BubblePool.Manager.DestroyMatches( this ) ;
	}

	public void OnDestroy()
	{
		if( BubblePool.Manager != null ) BubblePool.Manager.UnRegisterBubbles( this );
	}
}
