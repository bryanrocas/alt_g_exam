using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bubble : MonoBehaviour, IBubble, IBubbleDetect
{
	[SerializeField] int id;
	[SerializeField] BubbleColor bubbleColor;

	Rigidbody2D rb ;
	Collider2D col ;

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

	public Vector3 BubblePos
	{
		get
		{
			return transform.position ;
		}
	}

	public void Init (int id, BubbleColor bubbleColor)
	{
		ID = id;
		BubbleColor = bubbleColor;
	}
	#endregion





	public List<IBubble> GetMatchingNeighbors ()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll( new Vector2(transform.position.x , transform.position.y) , 
			Game.Manager.bubbleRadius , 1 << LayerMask.NameToLayer("Bubble") );

		List<IBubble> matchingNeighbors = new List<IBubble>();

		foreach( Collider2D col in cols )
		{
			IBubble bubble = col.GetComponent<IBubble>() ;
			if( bubble == null ) continue;
			if( bubble != this && bubble.BubbleColor == BubbleColor ) matchingNeighbors.Add( bubble );
		}

		//this.DebugError( "neighborCount: " + matchingNeighbors.Count + " ID " + this.ID ) ;

		return matchingNeighbors;
	}


	public void RegisterSelf()
	{
		BubblePool.Manager.ReigsterSelf( this );
	}

	public void RegisterNeighbors()
	{
		List<IBubble> matchingNeighbors = GetMatchingNeighbors() ;
		BubblePool.Manager.RegisterNeighbors( matchingNeighbors  );
	}

	bool toDestroy = false ;

	public void FallDown()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		rb.mass = Random.Range(0.2f,1f) ;
		rb.gravityScale = Random.Range(1,2f) ;
		rb.constraints = RigidbodyConstraints2D.None;
		col.isTrigger = true ;
		toDestroy = true ;
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		//this.DebugError( coll.gameObject.layer ) ;
		if( coll.gameObject.layer == LayerMask.NameToLayer("Wall") && toDestroy )
		{
			Destroy( gameObject ) ;
		}
	}
}
