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
				#if DEBUGGER
				foreach( IBubble bubble in chainedBubbles )
				{
					this.Debug( "chained removal: " + bubble.ID ) ;
				}
				#endif

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
		List<IBubble> saved = new List<IBubble>() ;
		List<IBubble> suspicion = new List<IBubble>() ;
		List<IBubble> destroyList = new List<IBubble>() ;

		// identify which among the casualties have 0 neighbors
		foreach( Bubble nearBubble in copyCasualty )
		{
			this.Debug( "casualties: " + nearBubble.ID + " :: " + 
				nearBubble.BubbleColor + " :: " + nearBubble.GetNeighbors().Count ) ;

			if( nearBubble.GetNeighbors().Count == 0 )
			{
				casualtyBubbles.Remove( nearBubble ) ;
				destroyList.Add( nearBubble ) ;
			}
		}

		copyCasualty = new List<IBubble>( casualtyBubbles ) ;

		foreach( Bubble copy in copyCasualty  )
		{
			bool notSafe = true ;
			IBubble stored = copy;

			foreach( IBubble bubble in copy.GetNeighbors() )
			{
				this.Debug( "casualtyCheck: " + 
					copy.ID + " :: " + 
					copy.BubbleColor + " :: " + 
					bubble.ID + " :: " + 
					bubble.BubbleColor + " :: " + 
					casualtyBubbles.Contains( bubble ) ) ;

				notSafe = casualtyBubbles.Contains( bubble ) ;

				if( !notSafe ) break ;
			}

			if( notSafe ) suspicion.Add( stored ) ;
			else saved.Add( stored ) ;
		}

		foreach( IBubble bubble in saved )
		{
			this.Debug( "saved-Check: " + bubble.ID + " " + bubble.BubbleColor ) ;
		}

		this.Debug( "suspicionCount: " + suspicion.Count ) ;

		List<IBubble> copiedSuspicion = new List<IBubble>( suspicion ) ;

		foreach( Bubble bubble in copiedSuspicion )
		{
			this.Debug( "suspicion-Check: " + bubble.ID + " " + bubble.BubbleColor ) ;
			bool lastChance = false ;

			foreach( IBubble saveBubble in saved )
			{
				lastChance = bubble.GetNeighbors().Contains( saveBubble ) ;

				if( lastChance ) break;
			}

			if( lastChance ) suspicion.Remove( bubble ) ;
		}

		foreach( Bubble bubble in suspicion )
		{
			this.Debug( "suspicion-Check-LAST: " + bubble.ID + " " + bubble.BubbleColor ) ;
			destroyList.Add( bubble ) ;
		}


		this.DebugError( "For Deletion: " + destroyList.Count ) ;

		foreach( IBubble bubble in destroyList )
		{
			this.Debug( "For Deletion: " +bubble.ID ) ;
			bubble.FallDown();
		}

	}
}
