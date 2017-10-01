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

	// 0 = any
	// 1 = same
	// 2 = opposite
	List<IBubble> MatchingNeighbors( int state , float bonusRadius = 0f )
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll( new Vector2(transform.position.x , transform.position.y) , 
			Game.Manager.bubbleRadius + bonusRadius , 1 << LayerMask.NameToLayer("Bubble") );

		List<IBubble> matchingNeighbors = new List<IBubble>();

		foreach( Collider2D col in cols )
		{
			IBubble bubble = col.GetComponent<IBubble>() ;
			if( bubble == null ) continue;

			switch( state )
			{
				case 0:
				if( bubble != this )matchingNeighbors.Add( bubble );
				break ;

				case 1:
				if( bubble != this && bubble.BubbleColor == BubbleColor ) matchingNeighbors.Add( bubble );
				break;
			
				case 2: 
				if( bubble.BubbleColor != BubbleColor ) matchingNeighbors.Add( bubble );
				break;
			}
		}

		return matchingNeighbors;
	}

	public List<IBubble> GetMatchingNeighbors ( float bonusRadius = 0f )
	{
		return MatchingNeighbors(1,bonusRadius);
	}

	public List<IBubble> GetOtherNeighbors ( float bonusRadius = 0f )
	{
		return MatchingNeighbors(2,bonusRadius);
	}

	public List<IBubble> GetNeighbors ( float bonusRadius = 0f )
	{
		return MatchingNeighbors(0,bonusRadius);
	}

	public void NotifyOtherNeighbors()
	{
		List<IBubble> matchingNeighbors = GetOtherNeighbors();

		Vector3 savedPos = transform.position ;

		foreach( IBubbleDetect bubble in matchingNeighbors )
		{
			this.Debug( "NotifyOtherNeighbors: " + ((IBubble)bubble).ID + " :: " + bubble.GetNeighbors().Count ); 
			if( bubble.GetNeighbors().Count <= 3 )
			{
				//this.DebugError( savedPos.magnitude + " " + bubble.BubblePos.magnitude + " " +bubble.ID ) ;

				bubble.CheckAnchors() ;
			}
		}
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

	public void CheckAnchors()
	{
		List<IBubble> matchingNeighbors = GetNeighbors() ;

		//this.DebugError( "ANCHOR CHECKED! " + ID + " " + matchingNeighbors.Count ) ;

		if( matchingNeighbors.Count == 0 ) FallDown(); // if nobody is supporting this, fall down
	}

	bool toDestroy = false ;

	public void FallDown()
	{
		gameObject.layer = 10; // set to destroyed layer
		
		//NotifyOtherNeighbors() ;

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
