using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : Singleton<BubblePool> 
{
	List<IBubble> chainedBubbles = new List<IBubble>() ;

	public void ReigsterSelf( IBubble bubble )
	{
		chainedBubbles.Clear() ;
		chainedBubbles = new List<IBubble>() ;
		chainedBubbles.Add( bubble );
		((IBubbleDetect)bubble).RegisterNeighbors() ;
	}

	public void RegisterNeighbors( List<IBubble> neighbors )
	{
		// if there are no neighbors detected, do not proceed
		if( neighbors.Count == 0)
		{
			this.Debug( "no neighbors detected"  ) ;
			return;
		}

		// if the newly added entry is of different color group, recreate the chain
		if( chainedBubbles.Count > 0 )
		{
			if( chainedBubbles[0].BubbleColor != neighbors[0].BubbleColor )
			{
				this.Debug( "neighbor of different color"  ) ;
				chainedBubbles = new List<IBubble>() ;
			}
		}

		// newly added neighbors are registered in the requireChain
		List<IBubble> requireChain = new List<IBubble>() ;
		int ctr = chainedBubbles.Count ;

		foreach( IBubble bubble in neighbors )
		{
			if( !chainedBubbles.Contains(bubble) )
			{
				chainedBubbles.Add( bubble ) ;
				requireChain.Add( bubble ) ;
				//this.Debug( "reqChain: " +  requireChain.Count + " ID " + bubble.ID ) ;
				ctr++;
			}
		}

		//this.DebugError( "FinalSequence: " + ctr + " " + requireChain.Count + " " + chainedBubbles.Count   ) ;

		// each requireChain will be required to register their neighbors
		foreach( IBubbleDetect bubble in requireChain )
		{
			bubble.RegisterNeighbors() ;
		}

		if( requireChain.Count == 0 && ctr == chainedBubbles.Count  )
		{
			if( ctr >= 3 ) DestroyBubbles();
		}
	}

	void DestroyBubbles()
	{
		foreach( Bubble bubble in chainedBubbles )
		{
			bubble.FallDown() ;
		}
	}


}
