using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : Singleton<BubblePool> 
{
	List<IBubble> chainedBubbles = new List<IBubble>() ;
	int ctr = 0 ;

	public void RegisterNeighbors( List<IBubble> neighbors )
	{
		// if the newly added entry is of different color group, remake the chain
		if( chainedBubbles.Count > 0 )
		{
			if( chainedBubbles[0].BubbleColor != neighbors[0].BubbleColor )
			{
				chainedBubbles = new List<IBubble>() ;
				ctr = 0;
			}
		}

		if( neighbors.Count <= 1) return;

		ctr++ ; //no. of times this method is called

		List<IBubble> requireChain = new List<IBubble>() ;

		foreach( IBubble bubble in neighbors )
		{
			if( !chainedBubbles.Contains(bubble) )
			{
				chainedBubbles.Add( bubble ) ;
				requireChain.Add( bubble ) ;
			}
		}

		this.DebugError( chainedBubbles.Count + " " + requireChain.Count + " " + ctr ) ;

		foreach( IBubbleDetect bubble in requireChain )
		{
			bubble.RegisterNeighbors() ;
		}


//		foreach( IBubble bubble in chainedBubbles )
//		{
//			this.Debug( ">> chainedBubbles: " + bubble.ID ) ;
//		}
//
//		foreach( IBubble bubble in requireChain )
//		{
//			this.Debug( ">> requireChain: " + bubble.ID ) ;
//		}


		if( requireChain.Count == 0 && ctr > chainedBubbles.Count && chainedBubbles.Count > 2 )
		{
			this.DebugError( ">>>>> FINAL COUNT: " +  chainedBubbles.Count + " " + requireChain.Count + " " + ctr ) ;

			DestroyBubbles() ;
		}
	}

	void DestroyBubbles()
	{
		foreach( Bubble bubble in chainedBubbles )
		{
			bubble.FallDown() ;
		}

		chainedBubbles = new List<IBubble>() ;
	}


}
