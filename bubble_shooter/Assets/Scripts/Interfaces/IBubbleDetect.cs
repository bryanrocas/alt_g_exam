using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBubbleDetect
{
	List<IBubble> GetNeighbors ( float bonusRadius = 0f );
	List<IBubble> GetMatchingNeighbors( float bonusRadius = 0f );
	List<IBubble> GetOtherNeighbors ( float bonusRadius = 0f );
	void NotifyOtherNeighbors();
	void RegisterSelf();
	void RegisterNeighbors();
	void CheckAnchors();
}

