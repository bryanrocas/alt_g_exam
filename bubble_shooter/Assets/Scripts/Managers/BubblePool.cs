using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : Singleton<BubblePool> 
{
	List<IBubble> chainedBubbles = new List<IBubble>() ;
	List<IBubble> casualtyBubbles = new List<IBubble>() ;

	public void ReigsterSelf( IBubble bubble )
	{
		casualtyBubbles.Clear() ;
		casualtyBubbles = new List<IBubble>() ;

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

				List<IBubble> nearby = ((IBubbleDetect)bubble).GetNeighbors() ;
				foreach( IBubble nearBubble in nearby )
				{
					if( !casualtyBubbles.Contains(nearBubble) ) casualtyBubbles.Add( nearBubble ) ;
				}

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
			if( ctr >= 3 )
			{
				DestroyBubbles();
				CheckCasualties() ;
			}
		}
	}

	void DestroyBubbles()
	{
		foreach( IBubble bubble in chainedBubbles )
		{
			if( casualtyBubbles.Contains(bubble) ) casualtyBubbles.Remove( bubble ) ;
			bubble.FallDown() ;
		}
	}

	void CheckCasualties()
	{
		List<IBubble> copyCasualty = new List<IBubble>( casualtyBubbles ) ;
		Dictionary<int, int> dict = new Dictionary<int, int>() ;
		List<IBubble> destroyList = new List<IBubble>() ;

		foreach( IBubble nearBubble in copyCasualty )
		{
			this.Debug( "casualties: " + nearBubble.ID + " :: " + 
				nearBubble.BubbleColor + " :: " + ((IBubbleDetect)nearBubble).GetNeighbors().Count ) ;

			if( ((IBubbleDetect)nearBubble).GetNeighbors().Count == 0 )
			{
				casualtyBubbles.Remove( nearBubble ) ;
				destroyList.Add( nearBubble ) ;
			}
		}

		copyCasualty = new List<IBubble>( casualtyBubbles ) ;

		foreach( IBubbleDetect copy in copyCasualty )
		{
			int ctr = 0 ;
			foreach( IBubbleDetect casualty in casualtyBubbles )
			{
				this.Debug( "casualtyCheck: " + ((IBubble)copy).ID + "  :: "  +
					((IBubble)casualty).ID + "  :: "  +
					casualty.GetNeighbors().Contains( ((IBubble)copy) )
				);
				if( casualty.GetNeighbors().Contains( ((IBubble)copy) ) ) ctr++ ;
			}
			if( ctr <= 1 ) dict.Add( ((IBubble)copy).ID , ctr ) ;
		}

		foreach( int key in dict.Keys )
		{
			this.Debug( "post-Check: " + key + " :: " + dict[key] ) ;
		}

		// final check
		foreach( int key in dict.Keys )
		{
			IBubble bubble = casualtyBubbles.Find( obj => obj.ID == key ) ;
			this.Debug( "final check: " + key + " :: " + ((IBubbleDetect)bubble).GetNeighbors().Count ) ;

			List<IBubble> neigh = ((IBubbleDetect)bubble).GetNeighbors() ;

			if( neigh.Count <= 1 ) 
			{
				foreach( IBubble neighB in neigh )
				{
					this.DebugError( "For Deletion - Neighborhood Check: " + bubble.ID +" :: "
						+ neighB.ID 
						+ " :: " + casualtyBubbles.Contains( neighB ) ) ;
				}
				destroyList.Add( bubble ) ;
			}
		}

		this.DebugError( "For Deletion: " + destroyList.Count ) ;

		foreach( IBubble bubble in destroyList )
		{
			this.Debug( "For Deletion: " +bubble.ID ) ;
		}
	}
}
