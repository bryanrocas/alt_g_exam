using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : Singleton<BubblePool> 
{
	List<IBubble> bubbles = new List<IBubble>();

	public void RegisterBubbles( IBubble bubble )
	{
		IBubble isFound = bubbles.Find( obj => obj == bubble );

		if( isFound == null ) bubbles.Add( bubble );
	}

	public void UnRegisterBubbles( IBubble bubble )
	{
		bubbles.Remove( bubble );
	}

	Dictionary<string,Dictionary<int,List<IBubble>>> chains = new Dictionary<string,Dictionary<int,List<IBubble>>>() ;

	public void RegisterNeighbors( IBubble bubble , List<IBubble> neighbors )
	{
		// we check if entry based on color has been made
		if( !chains.ContainsKey( bubble.BubbleColor.ToString() ) )
		{
			List<IBubble> entry = new List<IBubble>();
			entry.Add( bubble );

			Dictionary<int, List<IBubble>> entryDict = new Dictionary<int, List<IBubble>>() ;
			entryDict.Add( 0 , entry );

			chains.Add( bubble.BubbleColor.ToString() , entryDict );
		}
		// if already created, we check if IBubble is part of the list
		else
		{
			Dictionary<int, List<IBubble>> dict = chains[ bubble.BubbleColor.ToString() ] ;

			bool entryCreated = false ;

			// for each dict entry, we check if it contains the bubble
			for( int x = 0 ; x < dict.Count ; x++ )
			{
				entryCreated= dict[x].Contains( bubble ) ;

				if( entryCreated ) break;

				// if not, we check if it contains neighbors related to the bubble
				for( int y = 0 ; y < neighbors.Count ; y++ )
				{
					entryCreated = dict[x].Contains( neighbors[y] ) ;

					if( entryCreated )
					{
						// if it contains neighbors related to the bubble, we add the bubble to this list
						dict[x].Add( bubble ) ;
						break;
					}
				}

				if( entryCreated ) break;
			}

			//if by the end of the pass entryCreated is still false, we create a new entry
			if( !entryCreated )
			{
				List<IBubble> entry = new List<IBubble>();
				entry.Add( bubble );

				chains[bubble.BubbleColor.ToString()].Add( dict.Count , entry );
			}
		}

		#if DEBUG
		this.Debug( ">>>>>>> "+ chains.Count );

		foreach( string key in chains.Keys )
		{
			this.Debug( "ChainKey: " + key + " " + chains[key].Count ) ;
			foreach( int entrykey in chains[key].Keys )
			{
				this.Debug( "EntryKey: " + entrykey + " " + chains[key][entrykey].Count  ) ;

				foreach( IBubble entryBubble in chains[key][entrykey] )
				{
					this.Debug( "ListEntries: " + entryBubble.ID ) ;
				}
			}
		}
		#endif
	}

	// WIP:
	// the general idea is to get the neighbors linked to one node and check their neighbors
	// continue checking neighbors until a chain is formed
	// the problem is how to code this looping logic in an optimized manner
}
