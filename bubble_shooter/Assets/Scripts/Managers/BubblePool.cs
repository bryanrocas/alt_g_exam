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


		this.Debug( bubbles.Count );
	}

	public void UnRegisterBubbles( IBubble bubble )
	{
		bubbles.Remove( bubble );
	}

	List<IBubble> chain = new List<IBubble>();

	// find all bubbles with linking neighbors
	public void CheckMatchingNeighbors( int setId , BubbleColor setColor )
	{
		IBubble mainBubble = bubbles.Find( obj => obj.ID == setId && obj.BubbleColor == setColor ) ;

		List<IBubble> neighbors = ((IBubbleDetect)mainBubble).GetMatchingNeighbors() ;

		if( neighbors.Count == 0 ) return; //if mainBubble has no neighbors around, don't proceed

		chain = new List<IBubble>();
		chain.Add( mainBubble );
	}

	// WIP:
	// the general idea is to get the neighbors linked to one node and check their neighbors
	// continue checking neighbors until a chain is formed
	// the problem is how to code this looping logic in an optimized manner
}
