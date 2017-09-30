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

	public void InitRegistry()
	{
		foreach( IBubbleDetect bubble in bubbles )
		{
			bubble.RegisterNeighbors() ;
		}
	}

	Dictionary<string,Dictionary<int,List<IBubble>>> chains = new Dictionary<string,Dictionary<int,List<IBubble>>>() ;

	public void RegisterNeighbors( IBubble bubble , List<IBubble> neighbors )
	{
		int lastKeyValue = 0 ;

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
			int ctr = 0 ;

			// for each dict entry, we check if it contains the bubble
			//for( int x = 0 ; x < dict.Count ; x++ )
			foreach( int entrykey in dict.Keys )
			{
				lastKeyValue = entrykey;

				entryCreated= dict[entrykey].Contains( bubble ) ;

				if( entryCreated ) break;

				// if not, we check if it contains neighbors related to the bubble
				for( int y = 0 ; y < neighbors.Count ; y++ )
				{
					entryCreated = dict[entrykey].Contains( neighbors[y] ) ;

					if( entryCreated )
					{
						// if it contains neighbors related to the bubble, we add the bubble to this list
						dict[entrykey].Add( bubble ) ;
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

				chains[bubble.BubbleColor.ToString()].Add( lastKeyValue+1 , entry );
			}
		}

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
	}

	public void DestroyMatches( IBubble bubble )
	{
		Dictionary<int, List<IBubble>> dict = chains[ bubble.BubbleColor.ToString() ] ;
		int storedKey = 0 ;
		IBubble match = null ;

		foreach( int entrykey in dict.Keys )
		{
			match = dict[entrykey].Find( obj => obj == bubble ) ;
			if( match != null ) 
			{
				//this.DebugError( "Found: " +  entrykey + " " + dict[entrykey].Count ) ;
				storedKey = entrykey;
				break;
			}
		}

		if(dict[ storedKey ].Count <= 2 ) return;

		List<IBubble> bubblePops = new List<IBubble>(dict[ storedKey ]) ;

		dict.Remove( storedKey ) ;

		foreach( IBubble bubblePop in bubblePops )
		{
			Destroy( ((Bubble)bubblePop).gameObject ) ;
		}
	}
}
